namespace TeslaRent_Server.Helpers.CustomeExceptions
{
    public class DatabaseMigrationException : Exception
    {
        public DatabaseMigrationException(string message) : base(message) { }
    }
}
