using MediatR;

namespace TicketBookingApi.Features.Auth.RefreshToken
{
    public record RefreshTokenCommand (string Token) : IRequest<AuthResponseDto>;
}