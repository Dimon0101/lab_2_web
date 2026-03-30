using Lab_2_web.Data;
using Lab_2_web.Models;
using Lab_2_web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.RegularExpressions;

namespace Lab_2_web.Controllers
{
    public class BookingController : Controller
    {
        private readonly BookingService _bookingService;
        private readonly AppDbContext _context;

        public BookingController(BookingService bookingService, AppDbContext context)
        {
            _bookingService = bookingService;
            _context = context;
        }

        public IActionResult Index()
        {
            try
            {
                var bookings = _bookingService.GetAllBookings();
                return View(bookings);
            }
            catch
            {
                TempData["Error"] = "Помилка в опрацюванні запиту";
                return View(Enumerable.Empty<Booking>());
            }
        }

        public IActionResult Details(int id)
        {
            try
            {
                var booking = _bookingService.GetBookingById(id);
                if (booking == null) return NotFound();
                return View(booking);
            }
            catch
            {
                TempData["Error"] = "Помилка в опрацюванні запиту";
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Create()
        {
            LoadRoomDropdown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string visitorName, string visitorPhone,
                                    int roomId, DateTime checkInDate, DateTime checkOutDate)
        {
            ValidateBookingInput(visitorName, visitorPhone, roomId, checkInDate, checkOutDate);

            if (!ModelState.IsValid)
            {
                LoadRoomDropdown(roomId);
                RestoreViewBag(visitorName, visitorPhone, checkInDate, checkOutDate);
                return View();
            }

            try
            {
                if (HasRoomConflict(roomId, checkInDate, checkOutDate, excludeId: null))
                {
                    var room = _context.Rooms.Find(roomId);
                    ModelState.AddModelError("roomId", $"Кімната {room?.RoomNumber} вже заброньована на цей період.");
                    LoadRoomDropdown(roomId);
                    RestoreViewBag(visitorName, visitorPhone, checkInDate, checkOutDate);
                    return View();
                }

                var visitor = FindOrCreateVisitor(visitorName, visitorPhone);

                _bookingService.CreateBooking(new Booking
                {
                    VisitorId    = visitor.Id,
                    RoomId       = roomId,
                    CheckInDate  = checkInDate,
                    CheckOutDate = checkOutDate
                });

                TempData["Success"] = "Бронювання успішно створено";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Error"] = "Помилка в опрацюванні запиту";
                LoadRoomDropdown(roomId);
                RestoreViewBag(visitorName, visitorPhone, checkInDate, checkOutDate);
                return View();
            }
        }

        public IActionResult Edit(int id)
        {
            try
            {
                var booking = _bookingService.GetBookingById(id);
                if (booking == null) return NotFound();
                LoadRoomDropdown(booking.RoomId);
                return View(booking);
            }
            catch
            {
                TempData["Error"] = "Помилка в опрацюванні запиту";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, string visitorName, string visitorPhone,
                                   int roomId, DateTime checkInDate, DateTime checkOutDate)
        {
            var existingBooking = _bookingService.GetBookingById(id);
            if (existingBooking == null) return NotFound();

            ValidateBookingInput(visitorName, visitorPhone, roomId, checkInDate, checkOutDate);

            if (!ModelState.IsValid)
            {
                LoadRoomDropdown(roomId);
                ViewBag.VisitorName  = visitorName;
                ViewBag.VisitorPhone = visitorPhone;
                return View(existingBooking);
            }

            try
            {
                if (HasRoomConflict(roomId, checkInDate, checkOutDate, excludeId: id))
                {
                    var room = _context.Rooms.Find(roomId);
                    ModelState.AddModelError("roomId", $"Кімната {room?.RoomNumber} вже заброньована на цей період.");
                    LoadRoomDropdown(roomId);
                    ViewBag.VisitorName  = visitorName;
                    ViewBag.VisitorPhone = visitorPhone;
                    return View(existingBooking);
                }

                var visitor = FindOrCreateVisitor(visitorName, visitorPhone);

                existingBooking.VisitorId    = visitor.Id;
                existingBooking.RoomId       = roomId;
                existingBooking.CheckInDate  = checkInDate;
                existingBooking.CheckOutDate = checkOutDate;

                _bookingService.EditBooking(existingBooking);
                TempData["Success"] = "Бронювання успішно оновлено";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Error"] = "Помилка в опрацюванні запиту";
                LoadRoomDropdown(roomId);
                ViewBag.VisitorName  = visitorName;
                ViewBag.VisitorPhone = visitorPhone;
                return View(existingBooking);
            }
        }

        public IActionResult Delete(int id)
        {
            try
            {
                var booking = _bookingService.GetBookingById(id);
                if (booking == null) return NotFound();
                return View(booking);
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
                _bookingService.DeleteBooking(id);
                TempData["Success"] = "Бронювання успішно видалено";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Error"] = "Помилка в опрацюванні запиту";
                return RedirectToAction(nameof(Index));
            }
        }

        private static readonly Regex PhoneRegex =
            new Regex(@"^\+?[0-9\s\-\(\)]{7,15}$", RegexOptions.Compiled);

        private void ValidateBookingInput(string visitorName, string visitorPhone,
                                          int roomId, DateTime checkIn, DateTime checkOut)
        {
            if (string.IsNullOrWhiteSpace(visitorName))
                ModelState.AddModelError("visitorName", "Введіть ім'я відвідувача");

            if (!string.IsNullOrWhiteSpace(visitorPhone) && !PhoneRegex.IsMatch(visitorPhone.Trim()))
                ModelState.AddModelError("visitorPhone", "Введіть коректний номер телефону (наприклад, +380501234567)");

            if (roomId == 0)
                ModelState.AddModelError("roomId", "Оберіть кімнату");

            if (checkOut <= checkIn)
                ModelState.AddModelError("checkOutDate", "Дата виїзду має бути пізніше дати заїзду");
        }

        private bool HasRoomConflict(int roomId, DateTime checkIn, DateTime checkOut, int? excludeId)
        {
            return _context.Bookings.Any(b =>
                b.RoomId == roomId &&
                (excludeId == null || b.Id != excludeId) &&
                b.CheckInDate < checkOut &&
                b.CheckOutDate > checkIn);
        }

        private Visitor FindOrCreateVisitor(string name, string phone)
        {
            var visitor = _context.Visitors
                .FirstOrDefault(v => v.FullName.ToLower() == name.Trim().ToLower());

            if (visitor == null)
            {
                visitor = new Visitor
                {
                    FullName    = name.Trim(),
                    PhoneNumber = string.IsNullOrWhiteSpace(phone) ? null : phone.Trim()
                };
                _context.Visitors.Add(visitor);
                _context.SaveChanges();
            }

            return visitor;
        }

        private void LoadRoomDropdown(int selectedId = 0)
        {
            ViewBag.Rooms = new SelectList(
                _context.Rooms.OrderBy(r => r.RoomNumber),
                "Id", "RoomNumber", selectedId);
        }

        private void RestoreViewBag(string visitorName, string visitorPhone,
                                     DateTime checkIn, DateTime checkOut)
        {
            ViewBag.VisitorName  = visitorName;
            ViewBag.VisitorPhone = visitorPhone;
            ViewBag.CheckInDate  = checkIn.ToString("yyyy-MM-dd");
            ViewBag.CheckOutDate = checkOut.ToString("yyyy-MM-dd");
        }
    }
}
