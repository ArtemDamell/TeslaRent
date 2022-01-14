namespace TeslaCar_API.Helpers
{
    // 135. Создать в проекте API новую папку Helpers для хранения вспомогательных методов, в ней класс APISettings
    public class APISettings
    {
        public string SecretKey { get; set; }
        public string ValidAudience { get; set; }
        public string ValidIssuer { get; set; }
    }
}
