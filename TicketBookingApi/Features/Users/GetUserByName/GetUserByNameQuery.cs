using MediatR;

namespace TicketBookingApi.Features.Users.GetUserByName
{
    public record GetUserByNameQuery (string UserName) : IRequest<UserDto>;
}