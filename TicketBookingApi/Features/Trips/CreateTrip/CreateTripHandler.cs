using AutoMapper;
using MediatR;
using TicketBookingApi.Domain;
using TicketBookingApi.Infrastructure.Persistence;

namespace TicketBookingApi.Features.Trips.CreateTrip
{
    public class CreateTripHandler : IRequestHandler<CreateTripCommand, TripDto>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CreateTripHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TripDto> Handle(CreateTripCommand request, CancellationToken ct)
        {
            var trip = _mapper.Map<Trip>(request);
            await _context.Trips.AddAsync(trip, ct);
            await _context.SaveChangesAsync(ct);
            return _mapper.Map<TripDto>(trip);
        }
    }
}