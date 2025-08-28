using MediatR;

namespace TicketBookingApi.Features.Trips.CreateTrip
{
    public record CreateTripCommand(
        string From,
        string To,
        DateTime DepartureTime,
        DateTime ArrivalTime,
        int TotalSeats,
        decimal Price
    ) : IRequest<TripDto>;
}

