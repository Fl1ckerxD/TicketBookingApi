using FluentValidation;

namespace TicketBookingApi.Features.UserProfile.UpdateUser
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(u => u.LastName)
                .NotEmpty()
                .MaximumLength(30);

            RuleFor(u => u.Name)
                .NotEmpty()
                .MaximumLength(20);

            RuleFor(u => u.Patronymic)
                .MaximumLength(35);

            RuleFor(u => u.Email)
                .EmailAddress();
        }
    }
}