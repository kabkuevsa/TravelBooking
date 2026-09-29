namespace TravelBooking.Api.Models.DTOs.Flights;

public class CreateFlightRequest
{
    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public DateTime DepartureAt { get; set; }
    public DateTime ArrivalAt { get; set; }
    public string Airline { get; set; } = string.Empty;
    public string FlightNumber { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int SeatsAvailable { get; set; }
}