using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Data
{
    // 6.1 Реализовываем класс
    // 125. Указать новый класс пользователя, чтобы Entity Framework использовал его по умолчанию
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        // 6.2 Настраиваем параметры контекста данных
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {}

        // 16. Добавляем новую модель в контекст данных
        public DbSet<TeslaCar> TeslaCars {  get; set; }

        // 52. Добавляем таблицу картинок для машин
        public DbSet<TeslaCarImage> TeslaCarImages { get; set; }

        // 87. Домашнее задание
        public DbSet<CarAccessory> CarAccessories { get; set; }
    }
}
