using MediatR;
using Microsoft.AspNetCore.Identity;
using TicketBookingApi.Domain;

namespace TicketBookingApi.Features.Users.DeleteUser
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<DeleteUserHandler> _logger;

        public DeleteUserHandler(UserManager<User> userManager, ILogger<DeleteUserHandler> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            await _userManager.DeleteAsync(await _userManager.FindByNameAsync(request.UserName)
                ?? throw new KeyNotFoundException($"Пользователь с именем {request.UserName} не найден"));
            _logger.LogInformation($"Удален пользователь {request.UserName}");
        }
    }
}