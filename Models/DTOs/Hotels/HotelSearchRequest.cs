namespace TravelBooking.Api.Models.DTOs.Hotels;

public class HotelSearchRequest
{
    public string? City { get; set; }
    public int? MinStars { get; set; }
    public decimal? MaxPrice { get; set; }
}