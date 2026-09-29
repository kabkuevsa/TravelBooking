using Microsoft.EntityFrameworkCore;
using TravelBooking.Api.Data;
using TravelBooking.Api.Models.DTOs.Flights;
using TravelBooking.Api.Models.Entities;
using TravelBooking.Api.Services.Interfaces;

namespace TravelBooking.Api.Services.Implementations;

public class FlightService : IFlightService
{
    private readonly AppDbContext _db;

    public FlightService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<FlightDto>> SearchAsync(FlightSearchRequest r)
    {
        var query = _db.Flights.AsQueryable();

        if (!string.IsNullOrWhiteSpace(r.Origin))
            query = query.Where(f => f.Origin.Contains(r.Origin));

        if (!string.IsNullOrWhiteSpace(r.Destination))
            query = query.Where(f => f.Destination.Contains(r.Destination));

        if (r.DateFrom.HasValue)
            query = query.Where(f => f.DepartureAt >= r.DateFrom.Value);

        if (r.DateTo.HasValue)
            query = query.Where(f => f.DepartureAt <= r.DateTo.Value);

        if (r.MaxPrice.HasValue)
            query = query.Where(f => f.Price <= r.MaxPrice.Value);

        return await query
            .OrderBy(f => f.DepartureAt)
            .Select(f => ToDto(f))
            .ToListAsync();
    }

    public async Task<FlightDto?> GetByIdAsync(Guid id)
    {
        var flight = await _db.Flights.FindAsync(id);
        return flight is null ? null : ToDto(flight);
    }

    public async Task<FlightDto> CreateAsync(CreateFlightRequest r)
    {
        var flight = new Flight
        {
            Origin = r.Origin,
            Destination = r.Destination,
            DepartureAt = r.DepartureAt,
            ArrivalAt = r.ArrivalAt,
            Airline = r.Airline,
            FlightNumber = r.FlightNumber,
            Price = r.Price,
            SeatsAvailable = r.SeatsAvailable
        };

        _db.Flights.Add(flight);
        await _db.SaveChangesAsync();
        return ToDto(flight);
    }

    public async Task<bool> UpdateAsync(Guid id, CreateFlightRequest r)
    {
        var flight = await _db.Flights.FindAsync(id);
        if (flight is null) return false;

        flight.Origin = r.Origin;
        flight.Destination = r.Destination;
        flight.DepartureAt = r.DepartureAt;
        flight.ArrivalAt = r.ArrivalAt;
        flight.Airline = r.Airline;
        flight.FlightNumber = r.FlightNumber;
        flight.Price = r.Price;
        flight.SeatsAvailable = r.SeatsAvailable;

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var flight = await _db.Flights.FindAsync(id);
        if (flight is null) return false;

        _db.Flights.Remove(flight);
        await _db.SaveChangesAsync();
        return true;
    }

    private static FlightDto ToDto(Flight f) => new()
    {
        Id = f.Id,
        Origin = f.Origin,
        Destination = f.Destination,
        DepartureAt = f.DepartureAt,
        ArrivalAt = f.ArrivalAt,
        Airline = f.Airline,
        FlightNumber = f.FlightNumber,
        Price = f.Price,
        SeatsAvailable = f.SeatsAvailable
    };
}