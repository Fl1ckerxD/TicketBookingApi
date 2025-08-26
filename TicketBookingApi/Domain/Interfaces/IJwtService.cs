namespace TicketBookingApi.Domain.Interfaces
{
    public interface IJwtService
    {
        public string GenerateJwtToken(User user, IList<string> roles);
    }
}