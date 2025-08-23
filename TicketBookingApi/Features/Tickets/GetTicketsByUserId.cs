using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TicketBookingApi.Infrastructure.Persistence;

namespace TicketBookingApi.Features.Tickets
{
    public record GetTicketsByUserIdQuery(Guid Id) : IRequest<List<TicketDto>>;
    public class GetTicketsByUserIdHandler : IRequestHandler<GetTicketsByUserIdQuery, List<TicketDto>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public GetTicketsByUserIdHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<TicketDto>> Handle(GetTicketsByUserIdQuery request, CancellationToken ct)
        {
            var tickets = await _context.Tickets
                .Include(u => u.Trip)
                .Where(u => u.UserId == request.Id)
                .Select(u => _mapper.Map<TicketDto>(u))
                .ToListAsync(ct);

            return tickets;
        }
    }
}