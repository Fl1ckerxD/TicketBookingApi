namespace TicketBookingApi.Features.Auth.Register
{
    public class RegisterDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string? Patronymic { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}