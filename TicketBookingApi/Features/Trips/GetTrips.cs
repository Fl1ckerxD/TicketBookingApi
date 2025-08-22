using MediatR;
using Microsoft.EntityFrameworkCore;
using TicketBookingApi.Infrastructure.Persistence;

namespace TicketBookingApi.Features.Trips
{
    public record GetTripsQuery(string? From, string? To, DateTime? Date) : IRequest<List<TripDto>>;

    public record TripDto(int Id, string From, string To, DateTime Departure, DateTime Arrival, decimal Price);

    public class GetTripsHandler : IRequestHandler<GetTripsQuery, List<TripDto>>
    {
        private readonly AppDbContext _context;

        public GetTripsHandler(AppDbContext context) => _context = context;

        public async Task<List<TripDto>> Handle(GetTripsQuery request, CancellationToken ct)
        {
            var query = _context.Trips.AsQueryable();

            if (!string.IsNullOrEmpty(request.From))
                query = query.Where(t => t.From == request.From);

            if (!string.IsNullOrEmpty(request.To))
                query = query.Where(t => t.To == request.To);

            if (request.Date.HasValue)
                query = query.Where(t => t.DepartureTime.Date == request.Date.Value.Date);

            return await query
                .Select(t => new TripDto(t.Id, t.From, t.To, t.DepartureTime, t.ArrivalTime, t.Price))
                .ToListAsync(ct);
        }
    }
}