using MediatR;

namespace TicketBookingApi.Features.Tickets.GetTicketsByUser
{
    public record GetTicketsByUserIdQuery(Guid Id) : IRequest<List<TicketDto>>;
}