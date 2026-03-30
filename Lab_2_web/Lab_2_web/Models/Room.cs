using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Lab_2_web.Models
{
    public class Room
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Номер кімнати")]
        public string RoomNumber { get; set; }

        [Display(Name = "Місткість (осіб)")]
        public int Capacity { get; set; }

        [ValidateNever]
        public List<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
