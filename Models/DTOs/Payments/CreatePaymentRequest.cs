namespace TravelBooking.Api.Models.DTOs.Payments;

public class CreatePaymentRequest
{
    public Guid BookingId { get; set; }
    public string CardNumber { get; set; } = string.Empty;   
    public string CardHolder { get; set; } = string.Empty;
    public string Expiry { get; set; } = string.Empty;      
    public string Cvv { get; set; } = string.Empty;
}