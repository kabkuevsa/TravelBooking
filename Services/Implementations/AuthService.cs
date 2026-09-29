using Microsoft.EntityFrameworkCore;
using TravelBooking.Api.Data;
using TravelBooking.Api.Models.DTOs.Auth;
using TravelBooking.Api.Models.Entities;
using TravelBooking.Api.Services.Interfaces;

namespace TravelBooking.Api.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly IJwtService _jwt;
    private readonly INotificationService _notifications;

    public AuthService(AppDbContext db, IJwtService jwt, INotificationService notifications)
    {
        _db = db;
        _jwt = jwt;
        _notifications = notifications;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        if (await _db.Users.AnyAsync(u => u.Email == request.Email))
            throw new InvalidOperationException("Пользователь с таким email уже существует");

        var user = new User
        {
            Email = request.Email,
            FullName = request.FullName,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = "User"
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        await _notifications.SendEmailAsync(
            user.Id,
            user.Email,
            "Добро пожаловать в TravelBooking!",
            $"Здравствуйте, {user.FullName}! Ваш аккаунт успешно создан.");

        var (token, expiresAt) = _jwt.GenerateToken(user);
        return new AuthResponse
        {
            Token = token,
            ExpiresAt = expiresAt,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role
        };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == request.Email)
            ?? throw new InvalidOperationException("Неверный email или пароль");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new InvalidOperationException("Неверный email или пароль");

        var (token, expiresAt) = _jwt.GenerateToken(user);
        return new AuthResponse
        {
            Token = token,
            ExpiresAt = expiresAt,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role
        };
    }
}