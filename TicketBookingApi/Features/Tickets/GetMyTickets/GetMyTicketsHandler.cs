using MediatR;
using Microsoft.EntityFrameworkCore;
using TicketBookingApi.Infrastructure.Auth;
using TicketBookingApi.Infrastructure.Persistence;

namespace TicketBookingApi.Features.Tickets.GetMyTickets
{
    public class GetMyTicketsHandler : IRequestHandler<GetMyTicketsQuery, IEnumerable<TicketDto>>
    {
        private readonly AppDbContext _context;
        private readonly IUserContext _userContext;

        public GetMyTicketsHandler(AppDbContext context, IUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public async Task<IEnumerable<TicketDto>> Handle(GetMyTicketsQuery request, CancellationToken ct)
        {
            var tickets = await _context.Tickets.Where(t => t.UserId == _userContext.UserId)
                .Include(t => t.Trip)
                .Select(t => new TicketDto
                {
                    Id = t.Id,
                    From = t.Trip.From,
                    To = t.Trip.To,
                    SeatNumber = t.SeatNumber
                })
                .ToListAsync(ct);

            return tickets;
        }
    }
}