using TravelBooking.Api.Models.DTOs.Payments;

namespace TravelBooking.Api.Services.Interfaces;

public interface IPaymentService
{
    Task<PaymentDto> PayAsync(Guid userId, CreatePaymentRequest request);
    Task<PaymentDto?> GetByBookingIdAsync(Guid bookingId);
}