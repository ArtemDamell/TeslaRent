using Microsoft.AspNetCore.Components;
using Models;
using System.Web;
using TeslaRent_Client.Services.IServices;

namespace TeslaRent_Client.Pages.Authentication
{
    // 235. Реализовать логику компонента Login в Code Behind файле
    public partial class Login
    {
        [Inject] public IAuthenticationService _authService { get; set; }
        [Inject] public NavigationManager _navManager { get; set; }

        public bool IsProcessing { get; set; }
        public bool ShowAuthErrors { get; set; }
        public string? Error { get; set; }
        public string? ReturnUrl { get; set; }

        AuthenticationRequestDTO _authorizeInfo = new();

        async Task LoginUserHandler()
        {
            ShowAuthErrors = false;
            IsProcessing = true;

            var result = await _authService.Login(_authorizeInfo);
            if (result.IsAuthSuccessful)
            {
                IsProcessing = false;

                var absoluteUri = new Uri(_navManager.Uri);
                var queryParam = HttpUtility.ParseQueryString(absoluteUri.Query);
                ReturnUrl = queryParam["returnUrl"];
                if (string.IsNullOrWhiteSpace(ReturnUrl))
                    _navManager.NavigateTo("/");
                else
                    _navManager.NavigateTo($"/{ReturnUrl}");
            }
            else
            {
                IsProcessing = false;
                Error = result.ErrorMessage;
                ShowAuthErrors = true;
            }
        }
    }
}
