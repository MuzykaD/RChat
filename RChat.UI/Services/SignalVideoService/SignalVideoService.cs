using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using Radzen;
using RChat.Domain.Common;
using RChat.Domain.Messages.Dto;
using System.Security.Claims;

namespace RChat.UI.Services.SignalVideoService
{
    public class SignalVideoService : ISignalVideoService
    {
        private ILocalStorageService _localStorageService;
        private NotificationService _notificationService;
        private NavigationManager _navigationManager;
        private HubConnection _hubConnection;
        private AuthenticationStateProvider _authenticationStateProvider;
        private string _hubHostUrl;
        private int _currentUserId;

        public event Action OnCallTerminated;
        public event Action OnCallDeclined;

        public SignalVideoService(IConfiguration config,
            ILocalStorageService localStorageService,
            NavigationManager navigationManager,
            NotificationService notificationService,
            AuthenticationStateProvider provider)
        {
            _hubHostUrl = config["ApiHost"]!;
            _localStorageService = localStorageService;
            _notificationService = notificationService;
            _navigationManager = navigationManager;
            _authenticationStateProvider = provider;
        }

        public async Task StartAsync()
        {
            var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
            _currentUserId = int.Parse(state.User.FindFirstValue(ClaimTypes.NameIdentifier));
            _hubConnection = new HubConnectionBuilder().WithUrl($"{_hubHostUrl}/rVideoHub?userId={_currentUserId}",
                o => o.AccessTokenProvider =
                async () => await _localStorageService.GetItemAsync<string>("auth-jwt-token"))
                .Build();

            _hubConnection.On<int, string>("IncomingVideoCall", async (callerId, peerId) =>
            {
                NotificationMessage message = new()
                {
                    Summary = "Incoming video call!",
                    Severity = NotificationSeverity.Info,
                    Click = (notification) => AcceptCall(callerId, peerId),
                    CloseOnClick = true,
                };
            });
            


           await _hubConnection.StartAsync();
        }

        private  void AcceptCall(int callerId,  string peerId)
        {
            _navigationManager.NavigateTo("");
        }

        public HubConnection GetHub() => _hubConnection;
    }
}
