using AutoMapper;
using MediatR;
using TicketBookingApi.Infrastructure.Persistence;

namespace TicketBookingApi.Features.Trips
{
    public record GetTripQuery(int id) : IRequest<TripDto>;
    public class GetTripHandler : IRequestHandler<GetTripQuery, TripDto>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public GetTripHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TripDto> Handle(GetTripQuery request, CancellationToken ct)
        {
            var trip = await _context.Trips.FindAsync(request.id);
            return _mapper.Map<TripDto>(trip);
        }
    }
}