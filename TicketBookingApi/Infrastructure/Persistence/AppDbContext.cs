using Microsoft.EntityFrameworkCore;
using TicketBookingApi.Domain;

namespace TicketBookingApi.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Trip> Trips => Set<Trip>();
        public DbSet<Ticket> Tickets => Set<Ticket>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.User)
                .WithMany(u => u.Tickets)
                .HasForeignKey(t => t.UserId);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Trip)
                .WithMany(tr => tr.Tickets)
                .HasForeignKey(t => t.TripId);

            base.OnModelCreating(modelBuilder);
        }
    }
}