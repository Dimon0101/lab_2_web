using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Lab_2_web.Models
{
    public class Visitor
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введіть ім'я відвідувача")]
        [Display(Name = "ПІБ відвідувача")]
        public string FullName { get; set; }

        [Display(Name = "Номер телефону")]
        public string PhoneNumber { get; set; }

        [ValidateNever]
        public List<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
