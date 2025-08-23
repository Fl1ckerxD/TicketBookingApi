using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TicketBookingApi.Infrastructure.Persistence;

namespace TicketBookingApi.Features.Trips
{
    public record GetTripsQuery(string? From, string? To, DateTime? Date) : IRequest<List<TripDto>>;

    public class GetTripsHandler : IRequestHandler<GetTripsQuery, List<TripDto>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public GetTripsHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<TripDto>> Handle(GetTripsQuery request, CancellationToken ct)
        {
            var query = _context.Trips.AsQueryable();

            if (!string.IsNullOrEmpty(request.From))
                query = query.Where(t => t.From == request.From);

            if (!string.IsNullOrEmpty(request.To))
                query = query.Where(t => t.To == request.To);

            if (request.Date.HasValue)
                query = query.Where(t => t.DepartureTime.Date == request.Date.Value.Date);

            return _mapper.Map<List<TripDto>>(await query.ToListAsync(ct));
        }
    }
}