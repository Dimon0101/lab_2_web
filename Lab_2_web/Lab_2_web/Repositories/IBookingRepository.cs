using Lab_2_web.Models;

namespace Lab_2_web.Repositories
{
    public interface IBookingRepository
    {
        IEnumerable<Booking> GetAll();
        Booking GetById(int id);
        void Add(Booking booking);
        void Update(Booking booking);
        void Delete(int id);
        bool HasConflict(int roomId, DateTime checkIn, DateTime checkOut, int? excludeId);
    }
}
