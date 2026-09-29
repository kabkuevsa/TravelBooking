namespace TravelBooking.Api.Models.DTOs.Hotels;

public class CreateHotelRequest
{
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int Stars { get; set; }
    public decimal PricePerNight { get; set; }
    public int RoomsAvailable { get; set; }
}