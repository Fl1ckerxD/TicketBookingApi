using MediatR;
using TicketBookingApi.Infrastructure.Persistence;

namespace TicketBookingApi.Features.Trips.DeleteTrip
{
    public class DeleteTripHandler : IRequestHandler<DeleteTripCommand>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DeleteTripHandler> _logger;

        public DeleteTripHandler(AppDbContext context, ILogger<DeleteTripHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Handle(DeleteTripCommand request, CancellationToken ct)
        {
            var trip = await _context.Trips.FindAsync(request.Id, ct)
                ?? throw new KeyNotFoundException($"Поездка с идентификатором {request.Id} не найдена");
            _context.Trips.Remove(trip);
            await _context.SaveChangesAsync(ct);
            _logger.LogInformation($"Удалена поездка из {trip.From} в {trip.To}, Id: {trip.Id}");
        }
    }
}