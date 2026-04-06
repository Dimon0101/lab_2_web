using Lab_2_web.Data;
using Lab_2_web.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab_2_web.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly AppDbContext _context;

        public BookingRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Booking> GetAll()
        {
            return _context.Bookings
                .Include(b => b.Room)
                .ToList();
        }

        public Booking GetById(int id)
        {
            return _context.Bookings
                .Include(b => b.Room)
                .FirstOrDefault(b => b.Id == id);
        }

        public void Add(Booking booking)
        {
            _context.Bookings.Add(booking);
            _context.SaveChanges();
        }

        public void Update(Booking booking)
        {
            _context.Bookings.Update(booking);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var booking = _context.Bookings.Find(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                _context.SaveChanges();
            }
        }

        public bool HasConflict(int roomId, DateTime checkIn, DateTime checkOut, int? excludeId)
        {
            return _context.Bookings.Any(b =>
                b.RoomId == roomId &&
                (excludeId == null || b.Id != excludeId) &&
                b.CheckInDate < checkOut &&
                b.CheckOutDate > checkIn);
        }
    }
}
