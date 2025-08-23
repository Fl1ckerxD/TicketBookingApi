namespace TicketBookingApi.Features.Tickets
{
    public class TicketDto()
    {
        public Guid Id { get; set; }
        public string From { get; set; } = null!;
        public string To { get; set; } = null!;
        public int SeatNumber { get; set; }
    }
}