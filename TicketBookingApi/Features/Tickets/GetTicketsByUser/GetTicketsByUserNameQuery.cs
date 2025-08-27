using MediatR;

namespace TicketBookingApi.Features.Tickets.GetTicketsByUser
{
    public record GetTicketsByUserNameQuery(string UserName) : IRequest<List<TicketDto>>;
}