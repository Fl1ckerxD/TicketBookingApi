using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TicketBookingApi.Domain;
using TicketBookingApi.Domain.Interfaces;
using TicketBookingApi.Infrastructure.Persistence;

namespace TicketBookingApi.Features.Auth.RefreshToken
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, AuthResponseDto>
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IJwtService _jwtService;

        public RefreshTokenHandler(AppDbContext context, UserManager<User> userManager, IJwtService jwtService)
        {
            _context = context;
            _userManager = userManager;
            _jwtService = jwtService;
        }

        public async Task<AuthResponseDto> Handle(RefreshTokenCommand request, CancellationToken ct)
        {
            var storedToken = await _context.RefreshTokens.SingleOrDefaultAsync(rt => rt.Token == request.Token, ct);

            if (storedToken == null || !storedToken.IsActive)
                throw new UnauthorizedAccessException("Недействительный или просроченный токен обновления");

            var user = await _userManager.FindByIdAsync(storedToken.UserId.ToString());
            if (user == null)
                throw new UnauthorizedAccessException("Пользователь не найден");

            var roles = await _userManager.GetRolesAsync(user);

            var newAccessToken = _jwtService.GenerateJwtToken(user, roles);
            var newRefreshToken = _jwtService.GenerateRefreshToken(user.Id);

            storedToken.Revoked = DateTime.UtcNow;
            _context.RefreshTokens.Update(storedToken);
            await _context.RefreshTokens.AddAsync(newRefreshToken, ct);
            await _context.SaveChangesAsync(ct);

            return new AuthResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.Token
            };
        }
    }
}