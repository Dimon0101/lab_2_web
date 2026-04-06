using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Lab_2_web.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введіть ім'я відвідувача")]
        [Display(Name = "ПІБ відвідувача")]
        public string VisitorName { get; set; }

        [Display(Name = "Номер телефону")]
        public string? VisitorPhone { get; set; }

        [Required(ErrorMessage = "Оберіть дату заїзду")]
        [DataType(DataType.Date)]
        [Display(Name = "Дата заїзду")]
        public DateTime CheckInDate { get; set; }

        [Required(ErrorMessage = "Оберіть дату виїзду")]
        [DataType(DataType.Date)]
        [Display(Name = "Дата виїзду")]
        public DateTime CheckOutDate { get; set; }

        [Required(ErrorMessage = "Оберіть номер")]
        [Display(Name = "Номер")]
        public int RoomId { get; set; }

        [ValidateNever]
        public Room Room { get; set; }
    }
}
