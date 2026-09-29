namespace TravelBooking.Api.Models.Entities;

public class Flight
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Origin { get; set; } = string.Empty;       // Откуда
    public string Destination { get; set; } = string.Empty;  // Куда
    public DateTime DepartureAt { get; set; }
    public DateTime ArrivalAt { get; set; }
    public string Airline { get; set; } = string.Empty;
    public string FlightNumber { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int SeatsAvailable { get; set; }

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}