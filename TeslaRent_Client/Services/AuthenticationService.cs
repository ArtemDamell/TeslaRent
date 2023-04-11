using Blazored.LocalStorage;
using Common;
using Microsoft.AspNetCore.Components.Authorization;
using Models;
using Newtonsoft.Json;
using System.Text;
using TeslaRent_Client.Services.IServices;

namespace TeslaRent_Client.Services
{
    // 229.1 В папку Services клиентского проекта добавить новый класс AuthenticationService
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _client;
        private readonly ILocalStorageService _localStorage;

        // 239.1 Вносим изменения в AuthenticationService для использования новых методов
        private readonly AuthenticationStateProvider _authStatePrivider;


        public AuthenticationService(HttpClient client, ILocalStorageService localStorage, AuthenticationStateProvider authStatePrivider)
        {
            _client = client;
            _localStorage = localStorage;
            _authStatePrivider = authStatePrivider;
        }
        /// <summary>
        /// Logs in a user with the given authentication information and stores the authentication token and user details in the local storage.
        /// </summary>
        /// <param name="userAuthInfo">The authentication information of the user.</param>
        /// <returns>An authentication response containing the authentication status.</returns>
        public async Task<AuthenticationResponseDTO> Login(AuthenticationRequestDTO userAuthInfo)
        {
            // 229.3 Создаём HTTP Request
            var content = JsonConvert.SerializeObject(userAuthInfo);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("api/account/singin", bodyContent);
            var contentTemp = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<AuthenticationResponseDTO>(contentTemp);

            if (response.IsSuccessStatusCode)
            {
                await _localStorage.SetItemAsync(SD.LOCAL_TOKEN, result.Token);
                await _localStorage.SetItemAsync(SD.LOCAL_USER_DETAILS, result.User);

                // 239.3/.3 Вносим изменения в AuthenticationService для использования новых методов
                ((AuthStateProvider)_authStatePrivider).NotifyUserLoggedIn(result.Token);

                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", result.Token);
                return new AuthenticationResponseDTO { IsAuthSuccessful = true };
            }
            else
                return result;
        }

        // 230.1 Реализовываем LogOut метод в AuthenticationService
        /// <summary>
        /// Logs out the current user by removing the authentication token from local storage and notifying the authentication state provider.
        /// </summary>
        /// <returns>
        /// An asynchronous task that represents the operation.
        /// </returns>
        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync(SD.LOCAL_TOKEN);
            await _localStorage.RemoveItemAsync(SD.LOCAL_USER_DETAILS);

            // 239.2 Вносим изменения в AuthenticationService для использования новых методов
            ((AuthStateProvider)_authStatePrivider).NotifyUserLogout();

            _client.DefaultRequestHeaders.Authorization = null;
        }

        // // 230.2 Реализовываем RegisterUser метод в AuthenticationService
        /// <summary>
        /// Registers a user with the given user registration information.
        /// </summary>
        /// <param name="userRegInfo">The user registration information.</param>
        /// <returns>A <see cref="RegistrationResponseDTO"/> object indicating the success of the registration.</returns>
        public async Task<RegistrationResponseDTO> RegisterUser(UserRequestDTO userRegInfo)
        {
            var content = JsonConvert.SerializeObject(userRegInfo);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("api/account/singup", bodyContent);
            var contentTemp = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<RegistrationResponseDTO>(contentTemp);

            if (response.IsSuccessStatusCode)
            {
                return new RegistrationResponseDTO { IsRegistrationSuccessful = true };
            }
            else
                return result;
        }
    }
}
