using System.Diagnostics;
using MediatR;
using TicketBookingApi.Domain.Interfaces;
using TicketBookingApi.Infrastructure.Auth;

namespace TicketBookingApi.Features.Auth
{
    public record LoginCommand(string Username, string Password) : IRequest<string>;

    public class LoginHandler : IRequestHandler<LoginCommand, string>
    {
        private readonly IAuthService _authService;

        public LoginHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            return await _authService.LoginAsync(request.Username, request.Password);
        }
    }
}