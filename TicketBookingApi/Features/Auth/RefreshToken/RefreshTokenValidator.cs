using FluentValidation;

namespace TicketBookingApi.Features.Auth.RefreshToken
{
    public class RefreshTokenValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenValidator()
        {
            RuleFor(r => r.Token)
                .NotEmpty();
        }
    }
}