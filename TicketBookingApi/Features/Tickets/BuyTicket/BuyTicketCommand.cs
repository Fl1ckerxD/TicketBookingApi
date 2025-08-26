using MediatR;

namespace TicketBookingApi.Features.Tickets.BuyTicket
{
    public record BuyTicketCommand(int TripId, int SeatNumber) : IRequest<TicketDto>;
}