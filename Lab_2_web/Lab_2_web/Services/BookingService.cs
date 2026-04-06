using Lab_2_web.Models;
using Lab_2_web.Repositories;

namespace Lab_2_web.Services
{
    public class BookingService
    {
        private readonly IBookingRepository _repository;

        public BookingService(IBookingRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Booking> GetAllBookings() => _repository.GetAll();
        public Booking GetBookingById(int id) => _repository.GetById(id);
        public void CreateBooking(Booking booking) => _repository.Add(booking);
        public void EditBooking(Booking booking) => _repository.Update(booking);
        public void DeleteBooking(int id) => _repository.Delete(id);

        public bool HasRoomConflict(int roomId, DateTime checkIn, DateTime checkOut, int? excludeId)
            => _repository.HasConflict(roomId, checkIn, checkOut, excludeId);
    }
}
