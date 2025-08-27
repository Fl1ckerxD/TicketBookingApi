using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TicketBookingApi.Domain;
using TicketBookingApi.Domain.Interfaces;
using TicketBookingApi.Features.Auth.Register;

namespace TicketBookingApi.Infrastructure.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<AuthService> _logger;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;

        public AuthService(UserManager<User> userManager, IJwtService jwtService,
            SignInManager<User> signInManager, ILogger<AuthService> logger, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _jwtService = jwtService;
            _mapper = mapper;
        }

        public async Task<string> LoginAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                throw new UnauthorizedAccessException("Неверное имя пользователя или пароль");

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!result.Succeeded)
                throw new UnauthorizedAccessException("Неверное имя пользователя или пароль");

            var roles = await _userManager.GetRolesAsync(user);
            return _jwtService.GenerateJwtToken(user, roles);
        }

        public async Task RegisterAsync(RegisterDto registerDto)
        {
            var user = _mapper.Map<User>(registerDto);
            if (user == null)
                throw new ArgumentNullException(nameof(user), "User mapping resulted in null");
            if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(registerDto.Password))
                throw new ArgumentException("Необходимо указать имя пользователя и пароль");
            if (string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.LastName))
                throw new ArgumentException("Необходимо указать имя и фамилию");
            if (await _userManager.FindByNameAsync(user.UserName) != null)
                throw new ArgumentException("Пользователь с таким именем уже существует");

            await _userManager.CreateAsync(user, registerDto.Password);
            await _userManager.AddToRoleAsync(user, "User");
        }
    }
}