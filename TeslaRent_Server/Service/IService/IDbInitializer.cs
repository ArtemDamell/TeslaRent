namespace TeslaRent_Server.Service.IService
{
    // 114.1 Создаём интрерфейс для класса инициализации базы данных
    public interface IDbInitializer
    {
        Task Invoke(HttpContext httpContext);
    }
}
