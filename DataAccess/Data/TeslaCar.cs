using System.ComponentModel.DataAnnotations;

namespace DataAccess.Data
{
    // 14. Создаём класс описания машины Tesla для взаимодействия с базой данных
    // Модель, на основании которой будут создаваться сущности Entity Framework'а
    public class TeslaCar
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        // На сколько дней арендована
        [Required]
        public int Occupancy { get; set; }

        // Цена за аренду
        [Required]
        public double RegularRate { get; set; }
        public string Details { get; set; }

        // Количество мест в машине
        public string NumberOfSeats { get; set; }
        public string CreatedBy {get; set;}
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        // 53. Добавляем коллекцию для хранения картинок
        // Так, как свойство у нас виртуальное, миграции совершать не нужно!
        public virtual ICollection<TeslaCarImage>? TeslaCarImages { get; set; }
    }
}
