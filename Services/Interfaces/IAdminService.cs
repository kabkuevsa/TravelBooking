using TravelBooking.Api.Models.DTOs.Admin;

namespace TravelBooking.Api.Services.Interfaces;

public interface IAdminService
{
    Task<AdminStatsDto> GetStatsAsync();
    Task<IEnumerable<UserAdminDto>> GetUsersAsync();
    Task<bool> ChangeRoleAsync(Guid userId, string role);
    Task<bool> DeleteUserAsync(Guid userId);
}