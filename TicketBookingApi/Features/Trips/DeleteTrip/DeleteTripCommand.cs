using MediatR;

namespace TicketBookingApi.Features.Trips.DeleteTrip
{
    public record DeleteTripCommand(int Id) : IRequest;
}