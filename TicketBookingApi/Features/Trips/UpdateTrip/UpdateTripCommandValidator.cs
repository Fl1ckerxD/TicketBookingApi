using FluentValidation;

namespace TicketBookingApi.Features.Trips.UpdateTrip
{
    public class UpdateTripCommandValidator : AbstractValidator<UpdateTripCommand>
    {
        public UpdateTripCommandValidator()
        {
            RuleFor(x => x.From)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.To)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.TotalSeats)
                .InclusiveBetween(1, 300);

            RuleFor(x => x.Price)
                .InclusiveBetween(0.01m, 50000m);

            RuleFor(x => x.DepartureTime)
                .LessThan(x => x.ArrivalTime);
        }
    }
}