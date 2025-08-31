using FluentValidation;

namespace TicketBookingApi.Features.Auth.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(r => r.UserName)
                .NotEmpty()
                .MaximumLength(25);

            RuleFor(r => r.Password)
                .NotEmpty()
                .MinimumLength(6);

            RuleFor(r => r.LastName)
                .NotEmpty()
                .MaximumLength(30);

            RuleFor(r => r.Name)
                .NotEmpty()
                .MaximumLength(20);

            RuleFor(r => r.Patronymic)
                .MaximumLength(35);

            RuleFor(r => r.Email)
                .EmailAddress();
        }
    }
}