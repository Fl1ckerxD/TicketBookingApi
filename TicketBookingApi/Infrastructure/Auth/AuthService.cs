using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TicketBookingApi.Domain;
using TicketBookingApi.Domain.Interfaces;
using TicketBookingApi.Features.Auth;
using TicketBookingApi.Features.Auth.Register;
using TicketBookingApi.Infrastructure.Persistence;

namespace TicketBookingApi.Infrastructure.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly AppDbContext _context;
        private readonly ILogger<AuthService> _logger;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;

        public AuthService(UserManager<User> userManager, IJwtService jwtService,
            SignInManager<User> signInManager, AppDbContext context, ILogger<AuthService> logger, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _logger = logger;
            _jwtService = jwtService;
            _mapper = mapper;
        }

        public async Task<AuthResponseDto> ExternalLoginAsync(IEnumerable<Claim> claims, string provider)
        {
            var providerKey = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value
            ?? throw new Exception("Внешний провайдер не вернул NameIdentifier");

            var user = await _userManager.FindByLoginAsync(provider, providerKey);
            if (user == null)
            {
                var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value
                ?? throw new InvalidOperationException("Email is required for external login.");

                user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    var lastName = claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value;
                    var givenName = claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value;

                    user = new User
                    {
                        UserName = email ?? Guid.NewGuid().ToString("N"),
                        Email = email,
                        Name = givenName,
                        LastName = lastName ?? string.Empty,
                    };

                    var result = await _userManager.CreateAsync(user);
                    if (!result.Succeeded)
                        throw new Exception("Не удалось создать пользователя: " +
                            string.Join(", ", result.Errors.Select(e => e.Description)));
                    await _userManager.AddToRoleAsync(user, "User");
                }

                var loginInfo = new UserLoginInfo(provider, providerKey, provider);
                var addLoginResult = await _userManager.AddLoginAsync(user, loginInfo);
                if (!addLoginResult.Succeeded)
                    throw new Exception("Не удалось добавить внешний логин: " +
                        string.Join(", ", addLoginResult.Errors.Select(e => e.Description)));
            }

            var roles = await _userManager.GetRolesAsync(user);

            var accessToken = _jwtService.GenerateJwtToken(user, roles);
            var refreshToken = _jwtService.GenerateRefreshToken(user.Id);

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token
            };
        }

        public async Task<AuthResponseDto> LoginAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                throw new UnauthorizedAccessException("Неверное имя пользователя или пароль");

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!result.Succeeded)
                throw new UnauthorizedAccessException("Неверное имя пользователя или пароль");

            var roles = await _userManager.GetRolesAsync(user);

            var accessToken = _jwtService.GenerateJwtToken(user, roles);
            var refreshToken = _jwtService.GenerateRefreshToken(user.Id);

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token
            };
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