using Microsoft.EntityFrameworkCore;
using TravelBooking.Api.Data;
using TravelBooking.Api.Models.DTOs.Bookings;
using TravelBooking.Api.Models.Entities;
using TravelBooking.Api.Services.Interfaces;

namespace TravelBooking.Api.Services.Implementations;

public class BookingService : IBookingService
{
    private readonly AppDbContext _db;
    private readonly INotificationService _notifications;

    public BookingService(AppDbContext db, INotificationService notifications)
    {
        _db = db;
        _notifications = notifications;
    }

    public async Task<BookingDto> CreateAsync(Guid userId, CreateBookingRequest r)
    {
        if (r.FlightId is null && r.HotelId is null)
            throw new InvalidOperationException("Нужно указать рейс и/или отель");

        decimal total = 0;
        Flight? flight = null;
        Hotel? hotel = null;

        if (r.FlightId is not null)
        {
            flight = await _db.Flights.FindAsync(r.FlightId.Value)
                ?? throw new InvalidOperationException("Рейс не найден");
            if (flight.SeatsAvailable <= 0)
                throw new InvalidOperationException("Нет свободных мест на рейсе");
            total += flight.Price;
        }

        if (r.HotelId is not null)
        {
            hotel = await _db.Hotels.FindAsync(r.HotelId.Value)
                ?? throw new InvalidOperationException("Отель не найден");

            if (r.CheckIn is null || r.CheckOut is null)
                throw new InvalidOperationException("Для брони отеля нужны даты заезда и выезда");

            if (r.CheckOut <= r.CheckIn)
                throw new InvalidOperationException("Дата выезда должна быть позже даты заезда");

            if (hotel.RoomsAvailable <= 0)
                throw new InvalidOperationException("Нет свободных номеров");

            var nights = (r.CheckOut.Value - r.CheckIn.Value).Days;
            total += hotel.PricePerNight * nights;
        }

        var booking = new Booking
        {
            UserId = userId,
            FlightId = flight?.Id,
            HotelId = hotel?.Id,
            CheckIn = r.CheckIn,
            CheckOut = r.CheckOut,
            Status = BookingStatus.Pending,
            TotalPrice = total
        };

        if (flight is not null) flight.SeatsAvailable -= 1;
        if (hotel is not null) hotel.RoomsAvailable -= 1;

        _db.Bookings.Add(booking);
        await _db.SaveChangesAsync();

        await _notifications.SendEmailAsync(
            userId,
            $"user-{userId}@travel.local",
            "Бронь создана",
            $"Бронь #{booking.Id} на сумму {total} RUB создана.");

        return ToDto(booking);
    }

    public async Task<IEnumerable<BookingDto>> GetMyAsync(Guid userId)
    {
        return await _db.Bookings
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.CreatedAt)
            .Select(b => ToDto(b))
            .ToListAsync();
    }

    public async Task<IEnumerable<BookingDto>> GetAllAsync()
    {
        return await _db.Bookings
            .OrderByDescending(b => b.CreatedAt)
            .Select(b => ToDto(b))
            .ToListAsync();
    }

    public async Task<BookingDto?> GetByIdAsync(Guid id)
    {
        var b = await _db.Bookings.FindAsync(id);
        return b is null ? null : ToDto(b);
    }

    public async Task<bool> CancelAsync(Guid userId, Guid bookingId, bool isAdmin)
    {
        var booking = await _db.Bookings
            .Include(b => b.Flight)
            .Include(b => b.Hotel)
            .FirstOrDefaultAsync(b => b.Id == bookingId);

        if (booking is null) return false;

        if (!isAdmin && booking.UserId != userId)
            throw new UnauthorizedAccessException("Нельзя отменить чужую бронь");

        if (booking.Status == BookingStatus.Cancelled)
            throw new InvalidOperationException("Бронь уже отменена");

        booking.Status = BookingStatus.Cancelled;

        if (booking.Flight is not null) booking.Flight.SeatsAvailable += 1;
        if (booking.Hotel is not null) booking.Hotel.RoomsAvailable += 1;

        await _db.SaveChangesAsync();

        await _notifications.SendEmailAsync(
            booking.UserId,
            $"user-{booking.UserId}@travel.local",
            "Бронь отменена",
            $"Бронь #{booking.Id} отменена.");

        return true;
    }

    private static BookingDto ToDto(Booking b) => new()
    {
        Id = b.Id,
        UserId = b.UserId,
        FlightId = b.FlightId,
        HotelId = b.HotelId,
        CheckIn = b.CheckIn,
        CheckOut = b.CheckOut,
        Status = b.Status,
        TotalPrice = b.TotalPrice,
        CreatedAt = b.CreatedAt
    };
}