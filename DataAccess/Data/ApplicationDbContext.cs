using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Data
{
    // 6.1 Реализовываем класс
    // 125. Указать новый класс пользователя, чтобы Entity Framework использовал его по умолчанию
    // 175.1 Авторизации в серверном проекте мы получим ошибку.Вносим изменения в AppDbContext.
    public class ApplicationDbContext : IdentityDbContext/*<ApplicationUser>*/<IdentityUser>
    {
        // 6.2 Настраиваем параметры контекста данных
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        // 16. Добавляем новую модель в контекст данных
        public DbSet<TeslaCar> TeslaCars { get; set; }

        // 52. Добавляем таблицу картинок для машин
        public DbSet<TeslaCarImage> TeslaCarImages { get; set; }

        // 87. Домашнее задание
        public DbSet<CarAccessory> CarAccessories { get; set; }

        // 175.2 Добавляем DBSet для собственного класса
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        // 175.2 --> После этого создаём миграцию

        // 180. Добавить новую модель в контекст данных 
        public DbSet<CarOrderDetails> CarOrderDetails { get; set; }
    }
}