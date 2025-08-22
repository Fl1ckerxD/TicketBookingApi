using System.ComponentModel.DataAnnotations;

namespace TicketBookingApi.Domain
{
    public class User
    {

        public Guid Id { get; set; } = Guid.NewGuid();

        [MaxLength(30)]
        public string LastName { get; set; } = null!;

        [MaxLength(20)]
        public string Name { get; set; } = null!;

        [MaxLength(35)]
        public string? Patronymic { get; set; }

        [EmailAddress]
        [MaxLength(100)]
        public string? Email { get; set; }

        [Phone]
        [MaxLength(12)]
        public string? PhoneNumber { get; set; }

        [MaxLength(200)]
        public string PasswordHash { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        public string Role { get; set; } = "User";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();
    }
}