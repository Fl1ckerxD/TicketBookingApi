using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TicketBookingApi.Domain;

namespace TicketBookingApi.Infrastructure.Persistence
{
    public class SeedData
    {
        public static async Task SeedAsync(AppDbContext context, UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager, ILogger logger, int retry = 0)
        {
            var retryForAvailability = retry;
            try
            {
                if (context.Database.IsSqlServer())
                    context.Database.Migrate();

                if (!await context.Roles.AnyAsync())
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid> { Name = "User" });
                    await roleManager.CreateAsync(new IdentityRole<Guid> { Name = "Admin" });
                }

                if (!await context.Users.AnyAsync())
                {

                    var admin = new User
                    {
                        UserName = "log1",
                        Name = "Иван",
                        LastName = "Иванов",
                        Patronymic = "Иванович",
                        Email = "ivanov@mail.ru",
                        PhoneNumber = "+79991234567"
                    };
                    var user = new User
                    {
                        UserName = "log2",
                        Name = "Мария",
                        LastName = "Петрова",
                        Patronymic = "Сергеевна",
                        Email = "petrova@mail.ru",
                        PhoneNumber = "+79991234568"
                    };

                    await userManager.CreateAsync(admin, "admin123");
                    await userManager.CreateAsync(user, "user123");

                    await userManager.AddToRoleAsync(admin, "Admin");
                    await userManager.AddToRoleAsync(user, "User");
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
                    var users = context.Users.Take(2).ToArray();
                    var trips = context.Trips.Take(2).ToArray();

                    var tickets = new List<Ticket>
                    {
                        new Ticket { TripId = trips[0].Id, UserId = users[0].Id, SeatNumber = 1 },
                        new Ticket { TripId = trips[1].Id, UserId = users[0].Id, SeatNumber = 2 }
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
                await SeedAsync(context, userManager, roleManager, logger, retryForAvailability);
                throw;
            }
        }
    }
}