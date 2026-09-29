using TravelBooking.Api.Models.Entities;

namespace TravelBooking.Api.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (db.Flights.Any()) return;

        var flights = new List<Flight>
        {
            new() { Origin = "Москва", Destination = "Санкт-Петербург", DepartureAt = DateTime.UtcNow.AddDays(1), ArrivalAt = DateTime.UtcNow.AddDays(1).AddHours(1.5), Airline = "Аэрофлот", FlightNumber = "SU100", Price = 4500, SeatsAvailable = 50 },
            new() { Origin = "Москва", Destination = "Сочи", DepartureAt = DateTime.UtcNow.AddDays(2), ArrivalAt = DateTime.UtcNow.AddDays(2).AddHours(3), Airline = "S7", FlightNumber = "S7200", Price = 7800, SeatsAvailable = 30 },
            new() { Origin = "Казань", Destination = "Москва", DepartureAt = DateTime.UtcNow.AddDays(1), ArrivalAt = DateTime.UtcNow.AddDays(1).AddHours(2), Airline = "Победа", FlightNumber = "DP300", Price = 3200, SeatsAvailable = 80 },
            new() { Origin = "Москва", Destination = "Екатеринбург", DepartureAt = DateTime.UtcNow.AddDays(3), ArrivalAt = DateTime.UtcNow.AddDays(3).AddHours(2.5), Airline = "Уральские авиалинии", FlightNumber = "U6100", Price = 6500, SeatsAvailable = 40 },
            new() { Origin = "Санкт-Петербург", Destination = "Казань", DepartureAt = DateTime.UtcNow.AddDays(4), ArrivalAt = DateTime.UtcNow.AddDays(4).AddHours(3), Airline = "Аэрофлот", FlightNumber = "SU400", Price = 8200, SeatsAvailable = 25 }
        };

        db.Flights.AddRange(flights);
        await db.SaveChangesAsync();

        if (!db.Hotels.Any())
        {
            var hotels = new List<Hotel>
        {
            new() { Name = "Grand Hotel", City = "Москва", Address = "ул. Тверская, 1", Stars = 5, PricePerNight = 12000, RoomsAvailable = 20 },
            new() { Name = "Cosmos", City = "Москва", Address = "пр. Мира, 150", Stars = 4, PricePerNight = 6500, RoomsAvailable = 50 },
            new() { Name = "Nevsky Palace", City = "Санкт-Петербург", Address = "Невский пр., 10", Stars = 5, PricePerNight = 11000, RoomsAvailable = 15 },
            new() { Name = "Azimut", City = "Санкт-Петербург", Address = "наб. Обводного канала, 5", Stars = 4, PricePerNight = 5500, RoomsAvailable = 40 },
            new() { Name = "Kazan Inn", City = "Казань", Address = "ул. Баумана, 20", Stars = 3, PricePerNight = 3500, RoomsAvailable = 30 },
            new() { Name = "Sochi Beach", City = "Сочи", Address = "ул. Морская, 7", Stars = 4, PricePerNight = 7000, RoomsAvailable = 25 }
        };
            db.Hotels.AddRange(hotels);
            await db.SaveChangesAsync();
        }
    }


}