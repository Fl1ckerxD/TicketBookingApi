using AutoMapper;
using MediatR;
using TicketBookingApi.Domain.Interfaces;

namespace TicketBookingApi.Features.Auth.Register
{
    public class RegisterHandler : IRequestHandler<RegisterCommand>
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public RegisterHandler(IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        public async Task Handle(RegisterCommand request, CancellationToken ct)
        {
            var dto = _mapper.Map<RegisterDto>(request);
            await _authService.RegisterAsync(dto);
        }
    }
}