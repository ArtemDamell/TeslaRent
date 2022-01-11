using Common;
using DataAccess.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace TeslaCar_API.Controllers
{
    // 128.1 Добавить новый контроллер для управления учётными записями пользователя
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class AccountController : Controller
    {
        // 128.2 Внедряем необходимые классы для работы с ролями, пользователями и авторизацией
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        //130. Добавить конечную точку для регистрации пользователей
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SingUp([FromBody] UserRequestDTO userRequestDTO)
        {
            if (userRequestDTO is null || !ModelState.IsValid)
                return BadRequest();

            var user = new ApplicationUser
            {
                UserName = userRequestDTO.Email,
                Email = userRequestDTO.Email,
                FirstName = userRequestDTO.FirstName,
                LastName = userRequestDTO.LastName,
                PhoneNumber = userRequestDTO.Phone,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, userRequestDTO.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description);
                return BadRequest(new RegistrationResponseDTO
                {
                    Errors = errors,
                    IsRegistrationSuccessful = false
                });
            }

            if (!await _roleManager.RoleExistsAsync(SD.CUSTOMER_ROLE))
                await _roleManager.CreateAsync(new IdentityRole(SD.CUSTOMER_ROLE));

            var roleResult = await _userManager.AddToRoleAsync(user, SD.CUSTOMER_ROLE);
            if (!roleResult.Succeeded)
            {
                var errors = roleResult.Errors.Select(x => x.Description);
                return BadRequest(new RegistrationResponseDTO
                {
                    Errors = errors,
                    IsRegistrationSuccessful = false
                });
            }
            return StatusCode(201);
        }
    }
}
