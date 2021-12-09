using TeslaRent_Server.Service.IService;

namespace TeslaRent_Server.Helpers
{
    public static class DbInitializerExtension
    {
        public static IApplicationBuilder UseDbInitializer(this IApplicationBuilder app)
        {
            app.UseMiddleware<IDbInitializer>();
            return app;
        }
    }
}
