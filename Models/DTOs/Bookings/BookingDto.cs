using TravelBooking.Api.Models.Entities;

namespace TravelBooking.Api.Models.DTOs.Bookings;

public class BookingDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid? FlightId { get; set; }
    public Guid? HotelId { get; set; }
    public DateTime? CheckIn { get; set; }
    public DateTime? CheckOut { get; set; }
    public BookingStatus Status { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime CreatedAt { get; set; }
}