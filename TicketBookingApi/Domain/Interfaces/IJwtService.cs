namespace TicketBookingApi.Domain.Interfaces
{
    public interface IJwtService
    {
        string GenerateJwtToken(User user, IList<string> roles);
        RefreshToken GenerateRefreshToken(Guid userId);
    }
}