using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using RChat.UI.Services.BlazorAuthService;
using RChat.UI.Services.SignalClientService;

namespace RChat.UI.Pages.Authentication
{
    public partial class LogoutComponent : ComponentBase
    {
        [Inject]
        public IBlazorAuthService AuthService { get; set; }
        [CascadingParameter]
        protected ISignalClientService SignalClientService { get; set; }
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        public async Task Logout()
        {
            await AuthService.LogoutUserAsync();
            await SignalClientService.StopAsync();
            NavigationManager.NavigateTo("/");
        }

        public async Task Cancel()
        {
            NavigationManager.NavigateTo("/");
        }
    }
}
