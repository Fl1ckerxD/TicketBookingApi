using MediatR;

namespace TicketBookingApi.Features.Users.GetUsers
{
    public record GetUsersQuery() : IRequest<List<UserDto>>;
}