using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
