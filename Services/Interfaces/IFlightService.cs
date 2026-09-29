using TravelBooking.Api.Models.DTOs.Flights;

namespace TravelBooking.Api.Services.Interfaces;

public interface IFlightService
{
    Task<IEnumerable<FlightDto>> SearchAsync(FlightSearchRequest request);
    Task<FlightDto?> GetByIdAsync(Guid id);
    Task<FlightDto> CreateAsync(CreateFlightRequest request);
    Task<bool> UpdateAsync(Guid id, CreateFlightRequest request);
    Task<bool> DeleteAsync(Guid id);
}