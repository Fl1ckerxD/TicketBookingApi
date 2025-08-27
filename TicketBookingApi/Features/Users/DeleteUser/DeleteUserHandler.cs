using MediatR;
using Microsoft.AspNetCore.Identity;
using TicketBookingApi.Domain;

namespace TicketBookingApi.Features.Users.DeleteUser
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly UserManager<User> _userManager;

        public DeleteUserHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            await _userManager.DeleteAsync(await _userManager.FindByNameAsync(request.UserName)
                ?? throw new KeyNotFoundException($"Пользователь с именем {request.UserName} не найден"));
        }
    }
}