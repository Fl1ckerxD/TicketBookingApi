using AutoMapper;
using MediatR;
using TicketBookingApi.Infrastructure.Persistence;

namespace TicketBookingApi.Features.Trips.GetTripById
{
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
            var trip = await _context.Trips.FindAsync(request.id, ct)
                ?? throw new KeyNotFoundException($"Поездка с идентификатором {request.id} не найдена");
            return _mapper.Map<TripDto>(trip);
        }
    }
}