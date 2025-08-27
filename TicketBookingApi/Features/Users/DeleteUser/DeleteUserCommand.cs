using MediatR;

namespace TicketBookingApi.Features.Users.DeleteUser
{
    public record DeleteUserCommand(string UserName) : IRequest;
}