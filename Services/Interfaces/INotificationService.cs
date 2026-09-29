using TravelBooking.Api.Models.DTOs.Notifications;

namespace TravelBooking.Api.Services.Interfaces;

public interface INotificationService
{
    Task SendEmailAsync(Guid? userId, string recipient, string subject, string body);
    Task<IEnumerable<NotificationDto>> GetMyAsync(Guid userId);
    Task<IEnumerable<NotificationDto>> GetAllAsync();
}