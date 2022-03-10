using Microsoft.AspNetCore.Components;
using TeslaRent_Client.Services.IServices;

namespace TeslaRent_Client.Pages.Authentication
{
    public partial class Logout
    {
        [Inject] public IAuthenticationService AuthenticationService { get; set; }
        [Inject] public NavigationManager NavManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await AuthenticationService.Logout();
            NavManager.NavigateTo("/");
        }
    }
}
