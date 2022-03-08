using Models;

namespace TeslaRent_Client.Services.IServices
{
    // 228. В папку Services клиентского проекта добавить новый интерфейс IAuthenticationService
    public interface IAuthenticationService
    {
        Task<RegistrationResponseDTO> RegisterUser(UserRequestDTO userRegInfo);
        Task<AuthenticationResponseDTO> Login(AuthenticationRequestDTO userAuthInfo);
        Task Logout();
    }
}
