using MediatR;

namespace TicketBookingApi.Features.Auth.Login
{
    public record LoginCommand(string Username, string Password) : IRequest<AuthResponseDto>;
}