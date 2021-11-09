using Microsoft.EntityFrameworkCore;

namespace DataAccess.Data
{
    // 6.1 Реализовываем класс
    public class ApplicationDbContext : DbContext
    {
        // 6.2 Настраиваем параметры контекста данных
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {}

        // 16. Добавляем новую модель в контекст данных
        public DbSet<TeslaCar> TeslaCars {  get; set; }

        // 52. Добавляем таблицу картинок для машин
        public DbSet<TeslaCarImage> TeslaCarImages { get; set; }
    }
}
