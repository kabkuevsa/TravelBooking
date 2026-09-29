using TravelBooking.Api.Models.Entities;

namespace TravelBooking.Api.Services.Interfaces;

public interface IJwtService
{
    (string token, DateTime expiresAt) GenerateToken(User user);
}