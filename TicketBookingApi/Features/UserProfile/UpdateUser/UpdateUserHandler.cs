using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TicketBookingApi.Domain;
using TicketBookingApi.Infrastructure.Auth;

namespace TicketBookingApi.Features.UserProfile.UpdateUser
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UserProfileDto>
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserContext _userContext;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateUserHandler> _logger;

        public UpdateUserHandler(UserManager<User> userManager, IUserContext userContext,
            IMapper mapper, ILogger<UpdateUserHandler> logger)
        {
            _userManager = userManager;
            _userContext = userContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserProfileDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(_userContext.UserId.ToString())
            ?? throw new KeyNotFoundException($"Пользователь с идентификатором {_userContext.UserId} не найден");

            user.LastName = request.LastName;
            user.Name = request.Name;
            user.Patronymic = request.Patronymic;
            user.Email = request.Email;
            user.PhoneNumber = request.PhoneNumber;

            if (!string.IsNullOrEmpty(request.NewPassword))
            {
                if (!await _userManager.CheckPasswordAsync(user, request.Password))
                    throw new ArgumentException("Неверный пароль");

                var result = await _userManager.ChangePasswordAsync(user, request.Password, request.NewPassword);
                if (!result.Succeeded)
                    throw new Exception("Не удалось обновить пароль: " +
                        string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                string msg = "Не удалось обновить пользователя: " +
                    string.Join(", ", updateResult.Errors.Select(e => e.Description));
                _logger.LogError(msg);
                throw new Exception(msg);
            }

            _logger.LogInformation($"Пользователь {user.UserName}, обновил свой профиль");
            return _mapper.Map<UserProfileDto>(user);
        }
    }
}