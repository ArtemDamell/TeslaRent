using Microsoft.AspNetCore.Components;
using Models;
using System;
using TeslaRent_Client.Services.IServices;

namespace TeslaRent_Client.Pages.Authentication
{
    // 234. Рассмотрим, как переносить код в отдельный файл (Code Behind) на примере компонента Register, создать новый класс Register.razor.cs
    public partial class Register
    {
        [Inject] public IAuthenticationService _authService { get; set; }
        [Inject] public NavigationManager _navManager { get; set; }

        public bool IsProcessing { get; set; }
        public bool ShowRegistrationErrors { get; set; }
        public IEnumerable<string>? Errors { get; set; }

        UserRequestDTO UserForRegistration = new();

        async Task RegisterUserHandler()
        {
            ShowRegistrationErrors = false;
            IsProcessing = true;

            var result = await _authService.RegisterUser(UserForRegistration);
            if (result.IsRegistrationSuccessful)
            {
                IsProcessing = false;
                _navManager.NavigateTo("/login");
            }
            else
            {
                IsProcessing = false;
                Errors = result.Errors;
                ShowRegistrationErrors = true;
            }
        }
    }
}
