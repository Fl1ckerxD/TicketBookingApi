using MediatR;

namespace TicketBookingApi.Features.Auth.Register
{
    public record RegisterCommand
    (
        string UserName,
        string Password,
        string LastName,
        string Name,
        string? Patronymic,
        string? Email,
        string? PhoneNumber
    ) : IRequest;
}