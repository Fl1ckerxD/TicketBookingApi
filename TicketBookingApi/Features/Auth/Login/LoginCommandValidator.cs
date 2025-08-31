using FluentValidation;

namespace TicketBookingApi.Features.Auth.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(u => u.Username)
                .NotEmpty()
                .MaximumLength(25);

            RuleFor(u => u.Password)
                .NotEmpty()
                .MinimumLength(6);
        }
    }
}