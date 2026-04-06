using Lab_2_web.Models;

namespace Lab_2_web.Repositories
{
    public interface IRoomRepository
    {
        IEnumerable<Room> GetAll();
        Room GetById(int id);
        void Add(Room room);
        void Update(Room room);
        void Delete(int id);
        bool IsNumberTaken(string roomNumber, int? excludeId = null);
        bool HasBookings(int roomId);
    }
}
