using Lab_2_web.Data;
using Lab_2_web.Models;

namespace Lab_2_web.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly AppDbContext _context;

        public RoomRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Room> GetAll()
        {
            return _context.Rooms.OrderBy(r => r.RoomNumber).ToList();
        }

        public Room GetById(int id)
        {
            return _context.Rooms.Find(id);
        }

        public void Add(Room room)
        {
            _context.Rooms.Add(room);
            _context.SaveChanges();
        }

        public void Update(Room room)
        {
            _context.Rooms.Update(room);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var room = _context.Rooms.Find(id);
            if (room != null)
            {
                _context.Rooms.Remove(room);
                _context.SaveChanges();
            }
        }

        public bool IsNumberTaken(string roomNumber, int? excludeId = null)
        {
            return _context.Rooms.Any(r =>
                r.RoomNumber == roomNumber &&
                (excludeId == null || r.Id != excludeId));
        }

        public bool HasBookings(int roomId)
        {
            return _context.Bookings.Any(b => b.RoomId == roomId);
        }
    }
}
