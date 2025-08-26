using MediatR;

namespace TicketBookingApi.Features.Trips.GetTrips
{
    public record GetTripsQuery(string? From, string? To, DateTime? Date) : IRequest<List<TripDto>>;
}