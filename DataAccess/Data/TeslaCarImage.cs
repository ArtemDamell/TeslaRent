using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Data
{
    // 51. Создаём класс для обработки изображений
    public class TeslaCarImage
    {
        public int Id { get; set; }
        public string? CarImageUrl { get; set; }
        public int CarId { get; set; }

        [ForeignKey(nameof(CarId))]
        public virtual TeslaCar? TeslaCar { get; set; }
    }
}
