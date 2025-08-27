using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TicketBookingApi.Domain;
using TicketBookingApi.Infrastructure.Persistence;

namespace TicketBookingApi.Features.Tickets.GetTicketsByUser
{
    public class GetTicketsByUserNameHandler : IRequestHandler<GetTicketsByUserNameQuery, List<TicketDto>>
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public GetTicketsByUserNameHandler(UserManager<User> userManager, AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<List<TicketDto>> Handle(GetTicketsByUserNameQuery request, CancellationToken ct)
        {
            var user = await _userManager.FindByNameAsync(request.UserName)
                ?? throw new KeyNotFoundException($"Пользователь с именем {request.UserName} не найден");

            var tickets = await _context.Tickets
                .Include(u => u.Trip)
                .Where(u => u.UserId == user.Id)
                .Select(u => _mapper.Map<TicketDto>(u))
                .ToListAsync(ct);

            return tickets;
        }
    }
}