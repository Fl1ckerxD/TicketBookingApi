using MediatR;
using Microsoft.EntityFrameworkCore;
using TicketBookingApi.Domain;
using TicketBookingApi.Infrastructure.Auth;
using TicketBookingApi.Infrastructure.Persistence;

namespace TicketBookingApi.Features.Tickets.BuyTicket
{
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
            var trip = await _context.Trips.Include(t => t.Tickets).FirstOrDefaultAsync(t => t.Id == request.TripId, ct);
            if (trip == null)
                throw new KeyNotFoundException("Поездка не найдена");
            if (trip.SeatsAvailable <= 0)
                throw new Exception("Нет свободных мест");

            var ticket = new Ticket
            {
                UserId = _userContext.UserId.Value,
                TripId = trip.Id,
                SeatNumber = request.SeatNumber,
                PurchaseDate = DateTime.UtcNow
            };

            if(ticket.SeatNumber < 1 || ticket.SeatNumber > trip.TotalSeats)
                throw new Exception("Некорректный номер места");
            if (trip.Tickets.Any(t => t.SeatNumber == ticket.SeatNumber))
                throw new Exception("Место уже занято");

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync(ct);
            return new TicketDto { Id = ticket.Id, From = trip.From, To = trip.To, SeatNumber = ticket.SeatNumber };
        }
    }
}