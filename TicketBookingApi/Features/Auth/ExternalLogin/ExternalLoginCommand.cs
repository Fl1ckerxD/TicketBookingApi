using System.Security.Claims;
using MediatR;

namespace TicketBookingApi.Features.Auth.ExternalLogin
{
    public record ExternalLoginCommand(IEnumerable<Claim> Claims, string Provider) : IRequest<AuthResponseDto>;
}