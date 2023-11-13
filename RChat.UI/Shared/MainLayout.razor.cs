using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using Radzen;
using System.Security.Claims;

namespace RChat.UI.Shared
{
    public partial class MainLayoutComponent : LayoutComponentBase
    {
        [Inject]
        protected NotificationService NotificationService { get; set; }
        [Inject]
        protected NavigationManager NavigationManager { get; set; }
        [Inject]
        protected AuthenticationStateProvider StateProvider { get; set; }
        protected HubConnection _hubConnection;
        
        public bool IsConnected => _hubConnection.State.Equals(HubConnectionState.Connected);

        protected override async Task OnInitializedAsync()
        {/*
            _hubConnection = new HubConnectionBuilder().WithUrl("https://localhost:7089/rChatHub").Build();
            
            _hubConnection.On<string, string, string>("ReceiveChatNotification", async (message, receiverEmail, senderEmail)
                =>
            {
                var state = await StateProvider.GetAuthenticationStateAsync();
                var currentEmail = state.User.FindFirstValue(ClaimTypes.Email);
                if (currentEmail == receiverEmail)
                {
                    NotificationService.Notify(new()
                    {
                        Detail = message,
                        Severity = NotificationSeverity.Info,
                        Duration = 3000,
                        Click = (notification) =>
                        {
                            NavigationManager.NavigateTo($"/chats/private?email={senderEmail}");
                        }
                    });
                }
            });
            */
        }
    }
}
