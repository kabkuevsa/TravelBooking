using Microsoft.EntityFrameworkCore;
using TravelBooking.Api.Data;
using TravelBooking.Api.Models.DTOs.Hotels;
using TravelBooking.Api.Models.Entities;
using TravelBooking.Api.Services.Interfaces;

namespace TravelBooking.Api.Services.Implementations;

public class HotelService : IHotelService
{
    private readonly AppDbContext _db;

    public HotelService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<HotelDto>> SearchAsync(HotelSearchRequest r)
    {
        var query = _db.Hotels.AsQueryable();

        if (!string.IsNullOrWhiteSpace(r.City))
            query = query.Where(h => h.City.Contains(r.City));

        if (r.MinStars.HasValue)
            query = query.Where(h => h.Stars >= r.MinStars.Value);

        if (r.MaxPrice.HasValue)
            query = query.Where(h => h.PricePerNight <= r.MaxPrice.Value);

        return await query
            .OrderBy(h => h.PricePerNight)
            .Select(h => ToDto(h))
            .ToListAsync();
    }

    public async Task<HotelDto?> GetByIdAsync(Guid id)
    {
        var hotel = await _db.Hotels.FindAsync(id);
        return hotel is null ? null : ToDto(hotel);
    }

    public async Task<HotelDto> CreateAsync(CreateHotelRequest r)
    {
        var hotel = new Hotel
        {
            Name = r.Name,
            City = r.City,
            Address = r.Address,
            Stars = r.Stars,
            PricePerNight = r.PricePerNight,
            RoomsAvailable = r.RoomsAvailable
        };

        _db.Hotels.Add(hotel);
        await _db.SaveChangesAsync();
        return ToDto(hotel);
    }

    public async Task<bool> UpdateAsync(Guid id, CreateHotelRequest r)
    {
        var hotel = await _db.Hotels.FindAsync(id);
        if (hotel is null) return false;

        hotel.Name = r.Name;
        hotel.City = r.City;
        hotel.Address = r.Address;
        hotel.Stars = r.Stars;
        hotel.PricePerNight = r.PricePerNight;
        hotel.RoomsAvailable = r.RoomsAvailable;

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var hotel = await _db.Hotels.FindAsync(id);
        if (hotel is null) return false;

        _db.Hotels.Remove(hotel);
        await _db.SaveChangesAsync();
        return true;
    }

    private static HotelDto ToDto(Hotel h) => new()
    {
        Id = h.Id,
        Name = h.Name,
        City = h.City,
        Address = h.Address,
        Stars = h.Stars,
        PricePerNight = h.PricePerNight,
        RoomsAvailable = h.RoomsAvailable
    };
}