using TravelBooking.Api.Models.Entities;

namespace TravelBooking.Api.Models.DTOs.Notifications;

public class NotificationDto
{
    public Guid Id { get; set; }
    public string Recipient { get; set; } = string.Empty;
    public NotificationChannel Channel { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public NotificationStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}