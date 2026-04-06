using Lab_2_web.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab_2_web.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Room>().HasData(
                new Room { Id = 1, RoomNumber = "101",    Capacity = 2 },
                new Room { Id = 2, RoomNumber = "102",    Capacity = 3 },
                new Room { Id = 3, RoomNumber = "201",    Capacity = 1 },
                new Room { Id = 4, RoomNumber = "ЛЮКС-1", Capacity = 2 }
            );
        }
    }
}
