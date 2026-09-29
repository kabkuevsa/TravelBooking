namespace TravelBooking.Api.Models.Entities;

public enum BookingStatus
{
    Pending,
    Confirmed,
    Cancelled,
    Paid
}

public class Booking
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public User? User { get; set; }

    public Guid? FlightId { get; set; }
    public Flight? Flight { get; set; }

    public Guid? HotelId { get; set; }
    public Hotel? Hotel { get; set; }

    public DateTime? CheckIn { get; set; }
    public DateTime? CheckOut { get; set; }

    public BookingStatus Status { get; set; } = BookingStatus.Pending;
    public decimal TotalPrice { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Payment? Payment { get; set; }
}