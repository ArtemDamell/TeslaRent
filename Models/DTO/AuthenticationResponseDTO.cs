namespace Models
{
    // 131.2 Создать DTO модели для авторизации пользователей AuthenticationRequestDTO и AuthenticationResponseDTO
    public class AuthenticationResponseDTO
    {
        public bool IsAuthSuccessful { get; set; }
        public string ErrorMessage { get; set; }
        public string Token { get; set; }

        // На этом месте создаём модель UserDTO для хранения и передачи информации о авторизованном пользователе
        public UserDTO User { get; set; }
    }
}
