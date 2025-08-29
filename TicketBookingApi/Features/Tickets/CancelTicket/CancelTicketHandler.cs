using MediatR;
using TicketBookingApi.Infrastructure.Persistence;

namespace TicketBookingApi.Features.Tickets.CancelTicket
{
    public class CancelTicketHandler : IRequestHandler<CancelTicketCommand>
    {
        private readonly AppDbContext _context;

        public CancelTicketHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task Handle(CancelTicketCommand request, CancellationToken ct)
        {
            _context.Tickets.Remove(await _context.Tickets.FindAsync(request.Id, ct)
                ?? throw new KeyNotFoundException($"Билет с Id {request.Id} не найден"));
            await _context.SaveChangesAsync(ct);
        }
    }
}