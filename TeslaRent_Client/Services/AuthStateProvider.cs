using Blazored.LocalStorage;
using Common;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Net.Http.Headers;
using TeslaRent_Client.Helpers;

namespace TeslaRent_Client.Services
{
    // 222. В клиентскую часть в папку Services добавить новый сервис AuthStateProvider
    // Этот класс нам понадобится, чтобы глобально вставлять Bearer Token в каждый запрос
    public class AuthStateProvider :  AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public AuthStateProvider(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // На этом месте добавить константу Local_Token
            var token = await _localStorage.GetItemAsync<string>(SD.LOCAL_TOKEN);

            if (token is null)
                //226.Протестировать приложение(Для тестирования логики авторизованного пользователя, добавляем в класс AuthStateProvider пользователя вручную)
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            //return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(new []
            //{
            //    new Claim(ClaimTypes.Name, "john@gmail.com")
            //}, "jwtAuthType"
            //)));

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt((token)), "jwtAuthType")));
        }

        // 238. На данном этапе выход работает, но UI без обновления не меняется, исправим это, добавляем метод в AuthStateProvider
        public void NotifyUserLoggedIn(string token)
        {
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwtAuthType"));
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));

            NotifyAuthenticationStateChanged(authState);
        }
        public void NotifyUserLogout()
        {
            var authState = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
            NotifyAuthenticationStateChanged(authState);
        }

    }
}
