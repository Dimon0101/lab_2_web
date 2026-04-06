using Lab_2_web.Models;
using Lab_2_web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lab_2_web.Controllers
{
    public class RoomController : Controller
    {
        private readonly RoomService _roomService;

        public RoomController(RoomService roomService)
        {
            _roomService = roomService;
        }

        public IActionResult Index()
        {
            try
            {
                return View(_roomService.GetAllRooms());
            }
            catch
            {
                TempData["Error"] = "Помилка в опрацюванні запиту";
                return View(Enumerable.Empty<Room>());
            }
        }

        public IActionResult Details(int id)
        {
            try
            {
                var room = _roomService.GetRoomById(id);
                if (room == null) return NotFound();
                return View(room);
            }
            catch
            {
                TempData["Error"] = "Помилка в опрацюванні запиту";
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string roomNumber, int capacity)
        {
            ValidateInput(roomNumber, capacity);

            if (!ModelState.IsValid)
                return View();

            try
            {
                if (_roomService.IsNumberTaken(roomNumber))
                {
                    ModelState.AddModelError("roomNumber", $"Кімната з номером «{roomNumber}» вже існує.");
                    return View();
                }

                _roomService.CreateRoom(new Room
                {
                    RoomNumber = roomNumber.Trim(),
                    Capacity   = capacity
                });

                TempData["Success"] = "Кімнату успішно додано";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Error"] = "Помилка в опрацюванні запиту";
                return View();
            }
        }

        public IActionResult Edit(int id)
        {
            try
            {
                var room = _roomService.GetRoomById(id);
                if (room == null) return NotFound();
                return View(room);
            }
            catch
            {
                TempData["Error"] = "Помилка в опрацюванні запиту";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, string roomNumber, int capacity)
        {
            var room = _roomService.GetRoomById(id);
            if (room == null) return NotFound();

            ValidateInput(roomNumber, capacity);

            if (!ModelState.IsValid)
                return View(room);

            try
            {
                if (_roomService.IsNumberTaken(roomNumber, excludeId: id))
                {
                    ModelState.AddModelError("roomNumber", $"Кімната з номером «{roomNumber}» вже існує.");
                    return View(room);
                }

                room.RoomNumber = roomNumber.Trim();
                room.Capacity   = capacity;

                _roomService.EditRoom(room);
                TempData["Success"] = "Кімнату успішно оновлено";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Error"] = "Помилка в опрацюванні запиту";
                return View(room);
            }
        }

        public IActionResult Delete(int id)
        {
            try
            {
                var room = _roomService.GetRoomById(id);
                if (room == null) return NotFound();
                return View(room);
            }
            catch
            {
                TempData["Error"] = "Помилка в опрацюванні запиту";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                if (_roomService.HasBookings(id))
                {
                    TempData["Error"] = "Неможливо видалити кімнату — вона має активні бронювання.";
                    return RedirectToAction(nameof(Index));
                }

                _roomService.DeleteRoom(id);
                TempData["Success"] = "Кімнату успішно видалено";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Error"] = "Помилка в опрацюванні запиту";
                return RedirectToAction(nameof(Index));
            }
        }

        private void ValidateInput(string roomNumber, int capacity)
        {
            if (string.IsNullOrWhiteSpace(roomNumber))
                ModelState.AddModelError("roomNumber", "Введіть номер кімнати");

            if (capacity < 1)
                ModelState.AddModelError("capacity", "Місткість має бути не менше 1");
        }
    }
}
