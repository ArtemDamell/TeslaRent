namespace Models
{
    // 129.2 Теперь нам потребуется модель DTO для ответа сервера пользователю
    public class RegistrationResponseDTO
    {
        public bool IsRegistrationSuccessful { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}
