using System.ComponentModel.DataAnnotations;

namespace TicketBookingApi.Domain
{
    public class Ticket
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        [Required]
        public int TripId { get; set; }
        public Trip Trip { get; set; } = null!;

        [Range(1, 300)]
        public int SeatNumber { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; } = DateTime.Now;
    }
}