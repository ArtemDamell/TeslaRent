using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace TeslaRent_Client.Pages.Authentication
{
    // 255.2 Добавить функционал переадресации на страницу авторизации, если пользователь пытается получить доступ к закрытому компоненту
    // Для этого в папке Authentication создать новый компонент RedirectToLogin
    public partial class RedirectToLogin
    {
        [Inject] NavigationManager _navigationManager { get; set; }
        [CascadingParameter] Task<AuthenticationState> _authenticationState { get; set; }

        bool _notAuthorized;

        protected override async Task OnInitializedAsync()
        {
            var authState = await _authenticationState;
            if (authState?.User?.Identity is null || !authState.User.Identity.IsAuthenticated)
            {
                var returnUrl = _navigationManager.ToBaseRelativePath(_navigationManager.Uri);
                if (string.IsNullOrWhiteSpace(returnUrl))
                    _navigationManager.NavigateTo("login", true);
                else
                    _navigationManager.NavigateTo($"login?returnUrl={returnUrl}", true);
            }
            else
                _notAuthorized = true;
        }
    }
}
