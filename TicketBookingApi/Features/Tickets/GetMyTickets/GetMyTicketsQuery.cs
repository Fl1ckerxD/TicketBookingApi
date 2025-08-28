using MediatR;

namespace TicketBookingApi.Features.Tickets.GetMyTickets
{
    public record GetMyTicketsQuery() : IRequest<IEnumerable<TicketDto>>;
}