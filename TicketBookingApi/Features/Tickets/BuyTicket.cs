using MediatR;
using TicketBookingApi.Domain;
using TicketBookingApi.Features.Auth;
using TicketBookingApi.Infrastructure.Persistence;

namespace TicketBookingApi.Features.Tickets
{
    public record BuyTicketCommand(int TripId, int SeatNumber) : IRequest<TicketDto>;
    public class BuyTicketHandler : IRequestHandler<BuyTicketCommand, TicketDto>
    {
        private readonly AppDbContext _context;
        private readonly IUserContext _userContext;

        public BuyTicketHandler(AppDbContext context, IUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public async Task<TicketDto> Handle(BuyTicketCommand request, CancellationToken ct)
        {
            var trip = await _context.Trips.FindAsync(request.TripId, ct);
            if (trip == null || trip.SeatsAvailable <= 0)
                throw new Exception("Нет свободных мест");

            var ticket = new Ticket
            {
                UserId = _userContext.UserId.Value,
                TripId = trip.Id,
                SeatNumber = request.SeatNumber,
                PurchaseDate = DateTime.UtcNow
            };

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync(ct);

            return new TicketDto(ticket.Id, trip.From, trip.To, ticket.SeatNumber);
        }
    }

    public record TicketDto(Guid Id, string From, string To, int SeatNumber);
}