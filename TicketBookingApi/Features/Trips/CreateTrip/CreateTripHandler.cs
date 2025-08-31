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
        private readonly ILogger<CreateTripHandler> _logger;

        public CreateTripHandler(AppDbContext context, IMapper mapper, ILogger<CreateTripHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<TripDto> Handle(CreateTripCommand request, CancellationToken ct)
        {
            var trip = _mapper.Map<Trip>(request);
            await _context.Trips.AddAsync(trip, ct);
            await _context.SaveChangesAsync(ct);
            _logger.LogInformation($"Создана новая поездка из {trip.From} в {trip.To}, Id: {trip.Id}");
            return _mapper.Map<TripDto>(trip);
        }
    }
}