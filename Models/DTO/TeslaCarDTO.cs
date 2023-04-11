using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class TeslaCarDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter car name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter occupancy")]
        public int Occupancy { get; set; }

        [Range(1, 3000, ErrorMessage = "Regular rate must be between 1 and 3000")]
        public double RegularRate { get; set; }

        public string Details { get; set; }
        public string NumberOfSeats { get; set; }

        // 174. Добавить новые свойства в модель TeslaCarDTO
        public double TotalDays { get; set; }

        public double TotalAmount { get; set; }
        // 174.2 --> Идём в компонент TeslaCras и вносим правки

        // 61. Копируем свойство для картинки
        public virtual ICollection<TeslaCarImageDTO>? TeslaCarImages { get; set; }

        public List<string>? ImageUrls { get; set; }

        // 86. Домашнее задание
        public virtual ICollection<CarAccessoryDTO>? CarAccessories { get; set; }

        // 219.1 Добавить в модель TeslaCarDTO новое свойство, bool IsBooked
        // Далее в TeslaCarRepository в методе GetCar
        public bool IsBooked { get; set; }
        public string CreatedBy { get; set; }
    }
}