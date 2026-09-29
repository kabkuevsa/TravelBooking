namespace TravelBooking.Api.Models.DTOs.Admin;

public class AdminStatsDto
{
    public int TotalUsers { get; set; }
    public int TotalFlights { get; set; }
    public int TotalHotels { get; set; }
    public int TotalBookings { get; set; }
    public int PaidBookings { get; set; }
    public int CancelledBookings { get; set; }
    public decimal TotalRevenue { get; set; }
    public int TotalNotifications { get; set; }
}