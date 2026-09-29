using TravelBooking.Api.Models.DTOs.Bookings;

namespace TravelBooking.Api.Services.Interfaces;

public interface IBookingService
{
    Task<BookingDto> CreateAsync(Guid userId, CreateBookingRequest request);
    Task<IEnumerable<BookingDto>> GetMyAsync(Guid userId);
    Task<IEnumerable<BookingDto>> GetAllAsync();
    Task<bool> CancelAsync(Guid userId, Guid bookingId, bool isAdmin);
    Task<BookingDto?> GetByIdAsync(Guid id);
}