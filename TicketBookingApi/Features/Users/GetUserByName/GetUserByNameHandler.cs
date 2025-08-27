using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TicketBookingApi.Domain;

namespace TicketBookingApi.Features.Users.GetUserByName
{
    public class GetUserByNameHandler : IRequestHandler<GetUserByNameQuery, UserDto>
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public GetUserByNameHandler(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(GetUserByNameQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
                throw new KeyNotFoundException($"Пользователь с именем '{request.UserName}' не найден");

            var userDto = _mapper.Map<UserDto>(user);
            var roles = await _userManager.GetRolesAsync(user);
            userDto.Roles = roles;
            
            return userDto;
        }
    }
}