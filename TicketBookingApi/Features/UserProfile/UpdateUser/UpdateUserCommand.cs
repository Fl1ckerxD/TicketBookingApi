using MediatR;

namespace TicketBookingApi.Features.UserProfile.UpdateUser
{
    public record UpdateUserCommand(
        string? Password,
        string? NewPassword,
        string LastName,
        string Name,
        string? Patronymic,
        string? Email,
        string? PhoneNumber
    ) : IRequest<UserProfileDto>;
}