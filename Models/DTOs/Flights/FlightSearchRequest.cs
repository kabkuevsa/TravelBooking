namespace TravelBooking.Api.Models.DTOs.Flights;

public class FlightSearchRequest
{
    public string? Origin { get; set; }
    public string? Destination { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public decimal? MaxPrice { get; set; }
}