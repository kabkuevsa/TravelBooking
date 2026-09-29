using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelBooking.Api.Models.DTOs.Hotels;
using TravelBooking.Api.Services.Interfaces;

namespace TravelBooking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HotelsController : ControllerBase
{
    private readonly IHotelService _hotels;

    public HotelsController(IHotelService hotels)
    {
        _hotels = hotels;
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<HotelDto>>> Search([FromQuery] HotelSearchRequest request)
    {
        return Ok(await _hotels.SearchAsync(request));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<HotelDto>> GetById(Guid id)
    {
        var hotel = await _hotels.GetByIdAsync(id);
        return hotel is null ? NotFound() : Ok(hotel);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<HotelDto>> Create(CreateHotelRequest request)
    {
        var created = await _hotels.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, CreateHotelRequest request)
    {
        var ok = await _hotels.UpdateAsync(id, request);
        return ok ? NoContent() : NotFound();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var ok = await _hotels.DeleteAsync(id);
        return ok ? NoContent() : NotFound();
    }
}