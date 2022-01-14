using System.ComponentModel.DataAnnotations;

namespace Models
{
    // 129.1 Теперь нам потребуется модель DTO для регистрации пользователей
    public class UserRequestDTO
    {
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [RegularExpression("^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid email! Format is example@example.com")]
        public string Email { get; set; }
        public string? Phone { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm password is required")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password and confirm password is not matched")]
        public string ConfirmPassword { get; set; }
    }
}
