using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using RChat.UI.Services.BlazorAuthService;

namespace RChat.UI.Pages
{
    public partial class LogoutComponent : ComponentBase
    {
        [Inject]
        public IBlazorAuthService AuthService { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        public async Task Logout()
        {
            await AuthService.LogoutUserAsync();
            NavigationManager.NavigateTo("/");
        }

        public async Task Cancel()
        {
            NavigationManager.NavigateTo("/");
        }
    }
}
