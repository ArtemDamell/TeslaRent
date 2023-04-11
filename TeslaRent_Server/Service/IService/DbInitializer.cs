using Common;
using DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TeslaRent_Server.Helpers.CustomeExceptions;

namespace TeslaRent_Server.Service.IService
{
    // 114.2 Создаём класс реализации интерфейса
    public class DbInitializer : IDbInitializer
    {
        private readonly RequestDelegate _next;

        // 114.3 Внедряем все необходимые зависимости
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(RequestDelegate next, ApplicationDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _next = next;
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            // 114.4 Реализовываем метод инициализации
            try
            {
                // Проверяем, если есть хотя бы один файл миграций, применяем наш класс
                if (_db.Database.GetPendingMigrations().Any())
                    _db.Database.Migrate();
            }
            catch (Exception)
            {
                throw new DatabaseMigrationException("Error applying database migrations.");
            }

            // Проверяем роль и если её нет - создаём все роли и админа
            if (await _roleManager.RoleExistsAsync(SD.ADMIN_ROLE))
                return;

            await _roleManager.CreateAsync(new IdentityRole(SD.ADMIN_ROLE));
            await _roleManager.CreateAsync(new IdentityRole(SD.CUSTOMER_ROLE));
            await _roleManager.CreateAsync(new IdentityRole(SD.EMPLOYEE_ROLE));

            // Создаём пользователя с правами администратора
            var result = await _userManager.CreateAsync(new IdentityUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true
            }, "Qwerty123/");

            // Проверяем, создался ли пользователь
            if (result.Succeeded)
            {
                // Получаем объект добавленного пользователя
                var admin = await _db.Users.FirstOrDefaultAsync(x => x.Email.Equals("admin@gmail.com"));

                // Добавляем его к роли админа
                await _userManager.AddToRoleAsync(admin, SD.ADMIN_ROLE);
            }
            await _next(httpContext);
        }
    }
}
