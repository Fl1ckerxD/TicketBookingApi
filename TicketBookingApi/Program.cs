using System.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TicketBookingApi.Infrastructure.Persistence;
namespace TicketBookingApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        const string CONNECTION_STRING = "DefaultConnection";
        var conString = builder.Configuration.GetConnectionString(CONNECTION_STRING) ??
            throw new InvalidOperationException($"Connection string '{CONNECTION_STRING}' not found.");
        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddOpenApi();
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(conString));
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
        //builder.Services.AddAutoMapper(typeof(Program));

        var app = builder.Build();

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
