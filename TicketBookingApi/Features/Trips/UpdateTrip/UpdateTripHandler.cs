using AutoMapper;
using MediatR;
using TicketBookingApi.Infrastructure.Persistence;

namespace TicketBookingApi.Features.Trips.UpdateTrip
{
    public class UpdateTripHandler : IRequestHandler<UpdateTripCommand, TripDto>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public UpdateTripHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TripDto> Handle(UpdateTripCommand request, CancellationToken ct)
        {
            var trip = await _context.Trips.FindAsync(request.Id, ct);

            if (trip == null)
                throw new KeyNotFoundException($"Поездка с идентификатором {request.Id} не найдена");

            trip.From = request.From;
            trip.To = request.To;
            trip.DepartureTime = request.DepartureTime;
            trip.ArrivalTime = request.ArrivalTime;
            trip.TotalSeats = request.TotalSeats;
            trip.Price = request.Price;

            await _context.SaveChangesAsync(ct);

            return _mapper.Map<TripDto>(trip);
        }
    }
}