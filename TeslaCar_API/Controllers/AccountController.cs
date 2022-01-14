using Common;
using DataAccess.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TeslaCar_API.Helpers;

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

        // 137. Внедряем зависимость нового класса в AccountController
        private readonly APISettings _apiSettings;

        public AccountController(
            SignInManager<ApplicationUser> signInManager, 
            UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager,

            // 137. Внедряем зависимость нового класса в AccountController
            IOptions<APISettings> options
            )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _apiSettings = options.Value;
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

        // 133. Создаём конечные точки авторизации пользователей
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SingIn([FromBody] AuthenticationRequestDTO authenticationRequest)
        {
            var result = await _signInManager.PasswordSignInAsync(authenticationRequest.UserName, authenticationRequest.Password, false, false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(authenticationRequest.UserName);
                if (user is null)
                {
                    return Unauthorized(new AuthenticationResponseDTO
                        {
                            IsAuthSuccessful = false,
                            ErrorMessage = "Invalid authentication"
                        });
                }

                // Если всё прошло успешно, нам необходимо авторизовать пользователя
                // На этом месте переходим в AppSettings.json и добавляем необходимые данные

                var signingcredentials = GetSigningCredentials();
                var claims = await GetClaims(user);

                var tokenOptions = new JwtSecurityToken(
                        issuer: _apiSettings.ValidIssuer,
                        audience: _apiSettings.ValidAudience,
                        claims: claims,
                        expires: DateTime.UtcNow.AddDays(30),
                        signingCredentials: signingcredentials
                    );
                var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                return Ok(
                        new AuthenticationResponseDTO
                        {
                            IsAuthSuccessful = true,
                            Token = token,
                            User = new UserDTO
                            {
                                Id = user.Id,
                                FirstName = user.FirstName,
                                LastName = user.LastName,
                                Email = user.Email,
                                Phone = user.PhoneNumber
                            }
                        });
            }
            else
            {
                return Unauthorized(new AuthenticationResponseDTO
                {
                    IsAuthSuccessful = false,
                    ErrorMessage = "Invalid authentication"
                });
            }
        }

        // 138. GetSigningCredentials - Получить учетные данные для подписи
        SigningCredentials GetSigningCredentials()
        {
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_apiSettings.SecretKey));
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        // Объекты типа Claim ассациируются с пользователем в момент авторизации и сопровождают его до конца сессии
        async Task<List<Claim>> GetClaims(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("Id", user.Id)
            };

            // Так, как мы не получаем автоматически роли пользователя, нам необходимо получить её в ручную и ассациировать с пользователем
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }
    }
}
