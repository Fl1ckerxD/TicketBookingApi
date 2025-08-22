using Microsoft.EntityFrameworkCore;
using TicketBookingApi.Features.Auth;
using TicketBookingApi.Infrastructure.Persistence;
namespace TicketBookingApi;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        const string CONNECTION_STRING = "DefaultConnection";
        var conString = builder.Configuration.GetConnectionString(CONNECTION_STRING) ??
            throw new InvalidOperationException($"Connection string '{CONNECTION_STRING}' not found.");

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddOpenApi();
        builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(conString));
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
        builder.Services.AddAutoMapper(cfg => { }, typeof(Program));
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<IUserContext, UserContext>();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<AppDbContext>();
                await SeedData.SeedAsync(context, app.Logger);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "Error migrating database");
            }
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwaggerUi(options =>
            {
                options.DocumentPath = "/openapi/v1.json";
            });
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
