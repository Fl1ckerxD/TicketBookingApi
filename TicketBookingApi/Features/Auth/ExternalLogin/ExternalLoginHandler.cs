using MediatR;
using TicketBookingApi.Domain.Interfaces;

namespace TicketBookingApi.Features.Auth.ExternalLogin
{
    public class ExternalLoginHandler : IRequestHandler<ExternalLoginCommand, AuthResponseDto>
    {
        private readonly IAuthService _authService;

        public ExternalLoginHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<AuthResponseDto> Handle(ExternalLoginCommand request, CancellationToken cancellationToken)
        {
            var claims = request.Claims.ToList();
            return await _authService.ExternalLoginAsync(claims, request.Provider);
        }
    }

}