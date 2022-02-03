using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Common;

namespace Models.DTO
{
    public class CarOrderDetailsDTO
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        // ID сессии заказа
        [Required]
        public string StripeSessionId { get; set; }

        [Required]
        public DateTime StartRentDate { get; set; }

        [Required]
        public DateTime EndRentDate { get; set; }
        public DateTime ActualStartRentDate { get; set; }
        public DateTime ActualEndRentDate { get; set; }

        [Required]
        public double TotalCost { get; set; }
        public int TotalDays { get; set; }

        [Required]
        public int CarId { get; set; }
        public bool IsPaymentSuccessful { get; set; } = false;

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }
        public string Phone { get; set; }

        [ForeignKey(nameof(CarId))]
        public TeslaCarDTO TeslaCarDTO { get; set; }
        public Status Status { get; set; }
    }
}
