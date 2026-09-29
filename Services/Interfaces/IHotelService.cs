using TravelBooking.Api.Models.DTOs.Hotels;

namespace TravelBooking.Api.Services.Interfaces;

public interface IHotelService
{
    Task<IEnumerable<HotelDto>> SearchAsync(HotelSearchRequest request);
    Task<HotelDto?> GetByIdAsync(Guid id);
    Task<HotelDto> CreateAsync(CreateHotelRequest request);
    Task<bool> UpdateAsync(Guid id, CreateHotelRequest request);
    Task<bool> DeleteAsync(Guid id);
}