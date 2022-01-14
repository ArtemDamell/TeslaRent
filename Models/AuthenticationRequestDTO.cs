using System.ComponentModel.DataAnnotations;

namespace Models
{
    // 131.1 Создать DTO модели для авторизации пользователей AuthenticationRequestDTO и AuthenticationResponseDTO
    public class AuthenticationRequestDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [RegularExpression("^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid email! Format is example@example.com")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
