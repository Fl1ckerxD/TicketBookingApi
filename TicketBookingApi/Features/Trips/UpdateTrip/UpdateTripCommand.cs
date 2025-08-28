using MediatR;

namespace TicketBookingApi.Features.Trips.UpdateTrip
{
    public record UpdateTripCommand(
        int Id,
        string From,
        string To,
        DateTime DepartureTime,
        DateTime ArrivalTime,
        int TotalSeats,
        decimal Price
    ) : IRequest<TripDto>;
}