using Microsoft.EntityFrameworkCore;
using TravelBooking.Api.Data;
using TravelBooking.Api.Models.DTOs.Admin;
using TravelBooking.Api.Models.Entities;
using TravelBooking.Api.Services.Interfaces;

namespace TravelBooking.Api.Services.Implementations;

public class AdminService : IAdminService
{
    private readonly AppDbContext _db;

    public AdminService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<AdminStatsDto> GetStatsAsync()
    {
        return new AdminStatsDto
        {
            TotalUsers = await _db.Users.CountAsync(),
            TotalFlights = await _db.Flights.CountAsync(),
            TotalHotels = await _db.Hotels.CountAsync(),
            TotalBookings = await _db.Bookings.CountAsync(),
            PaidBookings = await _db.Bookings.CountAsync(b => b.Status == BookingStatus.Paid),
            CancelledBookings = await _db.Bookings.CountAsync(b => b.Status == BookingStatus.Cancelled),
            TotalRevenue = await _db.Payments
                .Where(p => p.Status == PaymentStatus.Succeeded)
                .SumAsync(p => (decimal?)p.Amount) ?? 0,
            TotalNotifications = await _db.Notifications.CountAsync()
        };
    }

    public async Task<IEnumerable<UserAdminDto>> GetUsersAsync()
    {
        return await _db.Users
            .OrderBy(u => u.CreatedAt)
            .Select(u => new UserAdminDto
            {
                Id = u.Id,
                Email = u.Email,
                FullName = u.FullName,
                Role = u.Role,
                CreatedAt = u.CreatedAt,
                BookingsCount = u.Bookings.Count
            })
            .ToListAsync();
    }

    public async Task<bool> ChangeRoleAsync(Guid userId, string role)
    {
        if (role != "User" && role != "Admin") return false;

        var user = await _db.Users.FindAsync(userId);
        if (user is null) return false;

        user.Role = role;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteUserAsync(Guid userId)
    {
        var user = await _db.Users
            .Include(u => u.Bookings)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null) return false;

        _db.Bookings.RemoveRange(user.Bookings);
        _db.Users.Remove(user);
        await _db.SaveChangesAsync();
        return true;
    }
}