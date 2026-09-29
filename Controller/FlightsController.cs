using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelBooking.Api.Models.DTOs.Flights;
using TravelBooking.Api.Services.Interfaces;

namespace TravelBooking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FlightsController : ControllerBase
{
    private readonly IFlightService _flights;

    public FlightsController(IFlightService flights)
    {
        _flights = flights;
    }

    // Публичный поиск
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<FlightDto>>> Search([FromQuery] FlightSearchRequest request)
    {
        return Ok(await _flights.SearchAsync(request));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<FlightDto>> GetById(Guid id)
    {
        var flight = await _flights.GetByIdAsync(id);
        return flight is null ? NotFound() : Ok(flight);
    }

    // Только админ
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<FlightDto>> Create(CreateFlightRequest request)
    {
        var created = await _flights.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, CreateFlightRequest request)
    {
        var ok = await _flights.UpdateAsync(id, request);
        return ok ? NoContent() : NotFound();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var ok = await _flights.DeleteAsync(id);
        return ok ? NoContent() : NotFound();
    }
}