using TeslaRent_Server.Service.IService;

namespace TeslaRent_Server.Helpers
{
    public static class DbInitializerExtension
    {
        /// <summary>
        /// Extension method to add middleware for initializing the database.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <returns>The application builder.</returns>
        public static IApplicationBuilder UseDbInitializer(this IApplicationBuilder app)
        {
            app.UseMiddleware<IDbInitializer>();
            return app;
        }
    }
}
