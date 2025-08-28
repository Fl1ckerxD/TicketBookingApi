using System.Text;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TicketBookingApi.Common.Validations;
using TicketBookingApi.Domain;
using TicketBookingApi.Domain.Interfaces;
using TicketBookingApi.Infrastructure.Auth;
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
        builder.Services.AddControllers()
            .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Program>());
        builder.Services.AddOpenApi();

        builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(conString));
        builder.Services.AddIdentity<User, IdentityRole<Guid>>(options =>
        {
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireDigit = false;
        })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders()
            .AddRoles<IdentityRole<Guid>>();

        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
        builder.Services.AddAutoMapper(cfg => { }, typeof(Program));
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<IUserContext, UserContext>();
        builder.Services.AddScoped<IJwtService, JwtService>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        // JWT config
        var jwtConfig = builder.Configuration.GetSection("Jwt");
        var key = Encoding.UTF8.GetBytes(jwtConfig["Key"]);

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtConfig["Issuer"],

                    ValidateAudience = true,
                    ValidAudience = jwtConfig["Audience"],

                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    ClockSkew = TimeSpan.Zero,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

        builder.Services.AddAuthorization();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<AppDbContext>();
                var userManager = services.GetRequiredService<UserManager<User>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
                await SeedData.SeedAsync(context, userManager, roleManager, app.Logger);
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

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
