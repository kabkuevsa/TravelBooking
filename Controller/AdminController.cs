using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelBooking.Api.Models.DTOs.Admin;
using TravelBooking.Api.Services.Interfaces;

namespace TravelBooking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _admin;

    public AdminController(IAdminService admin)
    {
        _admin = admin;
    }

    [HttpGet("stats")]
    public async Task<ActionResult<AdminStatsDto>> Stats()
    {
        return Ok(await _admin.GetStatsAsync());
    }

    [HttpGet("users")]
    public async Task<ActionResult<IEnumerable<UserAdminDto>>> Users()
    {
        return Ok(await _admin.GetUsersAsync());
    }

    [HttpPut("users/{id:guid}/role")]
    public async Task<IActionResult> ChangeRole(Guid id, [FromBody] ChangeRoleRequest request)
    {
        var ok = await _admin.ChangeRoleAsync(id, request.Role);
        return ok ? NoContent() : BadRequest(new { error = "Роль должна быть 'User' или 'Admin'" });
    }

    [HttpDelete("users/{id:guid}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var ok = await _admin.DeleteUserAsync(id);
        return ok ? NoContent() : NotFound();
    }
}

public class ChangeRoleRequest
{
    public string Role { get; set; } = string.Empty;
}