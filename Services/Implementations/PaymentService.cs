using Microsoft.EntityFrameworkCore;
using TravelBooking.Api.Data;
using TravelBooking.Api.Models.DTOs.Payments;
using TravelBooking.Api.Models.Entities;
using TravelBooking.Api.Services.Interfaces;

namespace TravelBooking.Api.Services.Implementations;

public class PaymentService : IPaymentService
{
    private readonly AppDbContext _db;
    private readonly INotificationService _notifications;

    public PaymentService(AppDbContext db, INotificationService notifications)
    {
        _db = db;
        _notifications = notifications;
    }

    public async Task<PaymentDto> PayAsync(Guid userId, CreatePaymentRequest r)
    {
        var booking = await _db.Bookings
            .Include(b => b.Payment)
            .FirstOrDefaultAsync(b => b.Id == r.BookingId)
            ?? throw new InvalidOperationException("Бронь не найдена");

        if (booking.UserId != userId)
            throw new UnauthorizedAccessException("Нельзя оплатить чужую бронь");

        if (booking.Status == BookingStatus.Cancelled)
            throw new InvalidOperationException("Нельзя оплатить отменённую бронь");

        if (booking.Status == BookingStatus.Paid)
            throw new InvalidOperationException("Бронь уже оплачена");

        if (booking.Payment is not null && booking.Payment.Status == PaymentStatus.Succeeded)
            throw new InvalidOperationException("Платёж уже проведён");

        var cardOk = IsCardValid(r.CardNumber, r.Expiry, r.Cvv);
        if (!cardOk)
            throw new InvalidOperationException("Карта отклонена (мок-шлюз)");

        var payment = new Payment
        {
            BookingId = booking.Id,
            Amount = booking.TotalPrice,
            Currency = "RUB",
            Status = PaymentStatus.Succeeded,
            TransactionId = $"MOCK-{Guid.NewGuid():N}".Substring(0, 20)
        };

        _db.Payments.Add(payment);
        booking.Status = BookingStatus.Paid;
        await _db.SaveChangesAsync();

        await _notifications.SendEmailAsync(
            booking.UserId,
            $"user-{booking.UserId}@travel.local",
            "Оплата прошла",
            $"Бронь #{booking.Id} оплачена. Сумма: {booking.TotalPrice} RUB. Транзакция: {payment.TransactionId}");

        return ToDto(payment);
    }

    public async Task<PaymentDto?> GetByBookingIdAsync(Guid bookingId)
    {
        var p = await _db.Payments.FirstOrDefaultAsync(x => x.BookingId == bookingId);
        return p is null ? null : ToDto(p);
    }

    private static bool IsCardValid(string number, string expiry, string cvv)
    {
        var digits = new string(number.Where(char.IsDigit).ToArray());
        if (digits.Length != 16) return false;
        if (cvv.Length != 3 || !cvv.All(char.IsDigit)) return false;

        var parts = expiry.Split('/');
        if (parts.Length != 2) return false;
        if (!int.TryParse(parts[0], out var month) || month < 1 || month > 12) return false;
        if (!int.TryParse(parts[1], out var year)) return false;

        var expDate = new DateTime(2000 + year, month, 1).AddMonths(1).AddDays(-1);
        return expDate >= DateTime.UtcNow.Date;
    }

    private static PaymentDto ToDto(Payment p) => new()
    {
        Id = p.Id,
        BookingId = p.BookingId,
        Amount = p.Amount,
        Currency = p.Currency,
        Status = p.Status,
        TransactionId = p.TransactionId,
        CreatedAt = p.CreatedAt
    };
}