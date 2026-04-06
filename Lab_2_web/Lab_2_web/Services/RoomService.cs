using Lab_2_web.Models;
using Lab_2_web.Repositories;

namespace Lab_2_web.Services
{
    public class RoomService
    {
        private readonly IRoomRepository _repository;

        public RoomService(IRoomRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Room> GetAllRooms() => _repository.GetAll();
        public Room GetRoomById(int id) => _repository.GetById(id);
        public void CreateRoom(Room room) => _repository.Add(room);
        public void EditRoom(Room room) => _repository.Update(room);
        public void DeleteRoom(int id) => _repository.Delete(id);
        public bool IsNumberTaken(string roomNumber, int? excludeId = null)
            => _repository.IsNumberTaken(roomNumber, excludeId);
        public bool HasBookings(int roomId)
            => _repository.HasBookings(roomId);
    }
}
