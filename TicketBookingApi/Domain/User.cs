using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace TicketBookingApi.Domain
{
    public class User : IdentityUser<Guid>
    {
        [Required]
        [MaxLength(25)]
        public override string UserName { get; set; } = null!;

        [MaxLength(30)]
        public string LastName { get; set; } = null!;

        [MaxLength(20)]
        public string Name { get; set; } = null!;

        [MaxLength(35)]
        public string? Patronymic { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();
    }
}