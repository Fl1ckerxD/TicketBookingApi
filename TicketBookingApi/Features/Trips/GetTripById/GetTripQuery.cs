using MediatR;

namespace TicketBookingApi.Features.Trips.GetTripById
{
    public record GetTripQuery(int id) : IRequest<TripDto>;
}