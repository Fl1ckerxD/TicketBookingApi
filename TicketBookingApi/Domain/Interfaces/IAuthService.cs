using System.Security.Claims;
using TicketBookingApi.Features.Auth;
using TicketBookingApi.Features.Auth.Register;

namespace TicketBookingApi.Domain.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(string username, string password);
        Task RegisterAsync(RegisterDto registerDto);
        Task<AuthResponseDto> ExternalLoginAsync(IEnumerable<Claim> claims, string provider);
    }
}