using MediatR;
using TicketBookingApi.Infrastructure.Persistence;

namespace TicketBookingApi.Features.Trips.DeleteTrip
{
    public class DeleteTripHandler : IRequestHandler<DeleteTripCommand>
    {
        private readonly AppDbContext _context;

        public DeleteTripHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteTripCommand request, CancellationToken ct)
        {
            _context.Trips.Remove(await _context.Trips.FindAsync(request.Id, ct)
                ?? throw new KeyNotFoundException($"Поездка с идентификатором {request.Id} не найдена"));
            await _context.SaveChangesAsync(ct);
        }
    }
}