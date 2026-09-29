using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelBooking.Api.Models.DTOs.Payments;
using TravelBooking.Api.Services.Interfaces;

namespace TravelBooking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _payments;

    public PaymentsController(IPaymentService payments)
    {
        _payments = payments;
    }

    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub")!);

    [HttpPost]
    public async Task<ActionResult<PaymentDto>> Pay(CreatePaymentRequest request)
    {
        try
        {
            var payment = await _payments.PayAsync(CurrentUserId, request);
            return Ok(payment);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("booking/{bookingId:guid}")]
    public async Task<ActionResult<PaymentDto>> GetByBooking(Guid bookingId)
    {
        var payment = await _payments.GetByBookingIdAsync(bookingId);
        return payment is null ? NotFound() : Ok(payment);
    }
}