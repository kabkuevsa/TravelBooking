using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelBooking.Api.Models.DTOs.Notifications;
using TravelBooking.Api.Services.Interfaces;

namespace TravelBooking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _notifications;

    public NotificationsController(INotificationService notifications)
    {
        _notifications = notifications;
    }

    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub")!);

    [HttpGet("my")]
    public async Task<ActionResult<IEnumerable<NotificationDto>>> My()
    {
        return Ok(await _notifications.GetMyAsync(CurrentUserId));
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<NotificationDto>>> All()
    {
        return Ok(await _notifications.GetAllAsync());
    }
}