using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketBookingApi.Domain;

namespace TicketBookingApi.Infrastructure.Persistence
{
    public class SeedData
    {
        public static async Task SeedAsync(AppDbContext context, ILogger logger, int retry = 0)
        {
            var retryForAvailability = retry;
            try
            {
                if (context.Database.IsSqlServer())
                    context.Database.Migrate();

                if (!await context.Users.AnyAsync())
                {
                    var users = new List<User>
                    {
                        new User { Name = "Иван", LastName = "Иванов", Patronymic = "Иванович", Email = "ivanov@mail.ru", PhoneNumber = "+79991234567", PasswordHash = "hash1", Role = "User" },
                        new User { Name = "Мария", LastName = "Петрова", Patronymic = "Сергеевна", Email = "petrova@mail.ru", PhoneNumber = "+79991234568", PasswordHash = "hash2", Role = "Admin" }
                    };
                    context.Users.AddRange(users);
                    await context.SaveChangesAsync();
                }

                if (!await context.Trips.AnyAsync())
                {
                    var trips = new List<Trip>
                    {
                        new Trip { From = "Москва", To = "Санкт-Петербург", DepartureTime = DateTime.Now.AddDays(1), ArrivalTime = DateTime.Now.AddDays(1).AddHours(3), TotalSeats = 2, Price = 1000 },
                        new Trip { From = "Казань", To = "Нижний Новгород", DepartureTime = DateTime.Now.AddDays(2), ArrivalTime = DateTime.Now.AddDays(2).AddHours(2), TotalSeats = 3, Price = 800 }
                    };
                    context.Trips.AddRange(trips);
                    await context.SaveChangesAsync();
                }

                if (!await context.Tickets.AnyAsync())
                {
                    var users = context.Users.ToArray();
                    var tickets = new List<Ticket>
                    {
                        new Ticket { UserId = users[0].Id, TripId = 1, SeatNumber = 1 },
                        new Ticket { UserId = users[1].Id, TripId = 2, SeatNumber = 2 }
                    };
                    context.Tickets.AddRange(tickets);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvailability >= 5) throw;

                retryForAvailability++;

                logger.LogError(ex.Message);
                await SeedAsync(context, logger, retryForAvailability);
                throw;
            }
        }
    }
}