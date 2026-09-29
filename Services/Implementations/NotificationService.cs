using Microsoft.EntityFrameworkCore;
using TravelBooking.Api.Data;
using TravelBooking.Api.Models.DTOs.Notifications;
using TravelBooking.Api.Models.Entities;
using TravelBooking.Api.Services.Interfaces;

namespace TravelBooking.Api.Services.Implementations;

public class NotificationService : INotificationService
{
    private readonly AppDbContext _db;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(AppDbContext db, ILogger<NotificationService> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task SendEmailAsync(Guid? userId, string recipient, string subject, string body)
    {
        var notification = new Notification
        {
            UserId = userId,
            Recipient = recipient,
            Channel = NotificationChannel.Email,
            Subject = subject,
            Body = body,
            Status = NotificationStatus.Queued
        };

        try
        {
            _logger.LogInformation(
                "📧 MOCK EMAIL → {Recipient} | Subject: {Subject}\n{Body}",
                recipient, subject, body);

            notification.Status = NotificationStatus.Sent;
        }
        catch (Exception ex)
        {
            notification.Status = NotificationStatus.Failed;
            notification.Error = ex.Message;
        }

        _db.Notifications.Add(notification);
        await _db.SaveChangesAsync();
    }

    public async Task<IEnumerable<NotificationDto>> GetMyAsync(Guid userId)
    {
        return await _db.Notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .Select(n => ToDto(n))
            .ToListAsync();
    }

    public async Task<IEnumerable<NotificationDto>> GetAllAsync()
    {
        return await _db.Notifications
            .OrderByDescending(n => n.CreatedAt)
            .Select(n => ToDto(n))
            .ToListAsync();
    }

    private static NotificationDto ToDto(Notification n) => new()
    {
        Id = n.Id,
        Recipient = n.Recipient,
        Channel = n.Channel,
        Subject = n.Subject,
        Body = n.Body,
        Status = n.Status,
        CreatedAt = n.CreatedAt
    };
}