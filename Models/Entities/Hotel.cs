namespace TravelBooking.Api.Models.Entities;

public class Hotel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int Stars { get; set; }
    public decimal PricePerNight { get; set; }
    public int RoomsAvailable { get; set; }

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}