using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using Radzen;
using RChat.UI.Services.SignalClientService;
using System.Security.Claims;

namespace RChat.UI.Shared
{
    public partial class MainLayoutComponent : LayoutComponentBase
    {
        [Inject]
        public ISignalClientService SignalClientService { get; set; }
        [Inject]
        public AuthenticationStateProvider StateProvider { get; set; }
        protected override async Task OnInitializedAsync()
        {
            var authenticationResult = await StateProvider.GetAuthenticationStateAsync();
            if (authenticationResult.User.Identity.IsAuthenticated)
            {
                await SignalClientService.StartAsync();
                await SignalClientService.RegisterUserSignalGroupsAsync();
            }
        }
    }
}
