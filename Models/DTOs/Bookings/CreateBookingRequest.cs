namespace TravelBooking.Api.Models.DTOs.Bookings;

public class CreateBookingRequest
{
    public Guid? FlightId { get; set; }
    public Guid? HotelId { get; set; }
    public DateTime? CheckIn { get; set; }
    public DateTime? CheckOut { get; set; }
}