using TicketBookingApi.Features.Auth;
using TicketBookingApi.Features.Auth.Register;

namespace TicketBookingApi.Domain.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(string username, string password);
        Task RegisterAsync(RegisterDto registerDto);
    }
}