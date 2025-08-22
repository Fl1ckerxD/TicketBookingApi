using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketBookingApi.Domain
{
    public class Trip
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string From { get; set; } = null!;

        [MaxLength(50)]
        public string To { get; set; } = null!;

        [Required]
        public DateTime DepartureTime { get; set; }

        [Required]
        public DateTime ArrivalTime { get; set; }

        [Range(1, 300)]
        public int TotalSeats { get; set; }

        [Range(0.01, 50000)]
        public decimal Price { get; set; }

        public ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();

        [NotMapped]
        public int SeatsAvailable => TotalSeats - Tickets.Count;
    }
}