using MediatR;

namespace TicketBookingApi.Features.Tickets.CancelTicket
{
    public record CancelTicketCommand (Guid Id) : IRequest;
}