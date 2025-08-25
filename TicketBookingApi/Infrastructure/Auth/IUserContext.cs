namespace TicketBookingApi.Infrastructure.Auth
{
    public interface IUserContext
    {
        Guid? UserId { get; }
        string? Role { get; }
        bool IsAuthenticated { get; }
    }
}