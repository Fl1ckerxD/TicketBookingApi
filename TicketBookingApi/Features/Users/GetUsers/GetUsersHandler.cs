using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TicketBookingApi.Domain;

namespace TicketBookingApi.Features.Users.GetUsers
{
    public class GetUsersHandler : IRequestHandler<GetUsersQuery, List<UserDto>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public GetUsersHandler(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<List<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userManager.Users.ToListAsync();
            var userDtos = _mapper.Map<List<UserDto>>(users);

            for (int i = 0; i < users.Count; i++)
            {
                var roles = await _userManager.GetRolesAsync(users[i]);
                userDtos[i].Roles = roles;
            }

            return userDtos;
        }
    }
}