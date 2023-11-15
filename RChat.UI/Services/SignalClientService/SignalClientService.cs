using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Radzen;
using RChat.Domain.Common;
using RChat.Domain.Messages;
using RChat.Domain.Messages.Dto;
using RChat.UI.ViewModels.InformationViewModels;

namespace RChat.UI.Services.SignalClientService
{
    //Dispose
    public class SignalClientService : ISignalClientService
    {
        private HubConnection? _hubConnection;
        public event Action<MessageInformationDto> OnMessageReceived;
        public event Action<MessageInformationDto> OnMessageDelete;
        private NotificationService _notificationService;
        private NavigationManager _navigationManager;
        private ILocalStorageService _localStorageService;
        private readonly string _hubHostUrl;
        public SignalClientService(
            NavigationManager navigationManager,
            NotificationService notificationService, 
            IConfiguration config,
            ILocalStorageService localStorageService)
        {
            _navigationManager = navigationManager;
            _notificationService = notificationService;
            _localStorageService = localStorageService;
            _hubHostUrl = config["ApiHost"]!;
        }
        private bool IsConnected => _hubConnection.State == HubConnectionState.Connected;
        public async Task CallSendMessageAsync(MessageInformationDto messageDto, NotificationArguments notificationArguments)
        {
            if (_hubConnection == null || !IsConnected)
                return;
            await _hubConnection.SendAsync("SendMessageAsync", messageDto, notificationArguments);
        }

        public async Task StartAsync()
        {
            _hubConnection = new HubConnectionBuilder().WithUrl($"{_hubHostUrl}/rChatHub",
                o => o.AccessTokenProvider = 
                async () => await _localStorageService.GetItemAsync<string>("auth-jwt-token"))
                .Build();

            _hubConnection.On<MessageInformationDto>("ReceiveMessage", message => OnMessageReceived?.Invoke(message));
            _hubConnection.On<MessageInformationDto>("OnMessageDelete", message => OnMessageDelete?.Invoke(message));
            _hubConnection.On<NotificationArguments>("ReceiveNotification", (args) =>
            {
                var navigationLink = args.IsGroup ? $"group?groupId={args.ReferenceId}" : $"private?userId={args.ReferenceId}";
                _notificationService.Notify(new()
                {
                    Summary = $"New message in {args.ChatName}",
                    Severity = NotificationSeverity.Info,
                    Duration = 2000,
                    Click = (notification) => _navigationManager.NavigateTo($"/chats/{navigationLink}")
                });
            });
            await _hubConnection.StartAsync();
        }

        public async Task JoinChatGroupAsync(int chatId)
        {           
            if (_hubConnection == null || !IsConnected)
                return;
            await _hubConnection.SendAsync("EnterChatGroupAsync", chatId);
        }

        public async Task LeaveChatGroupAsync(int chatId)
        {
            if (_hubConnection == null || !IsConnected)
                return;
            await _hubConnection.SendAsync("LeaveChatGroupAsync", chatId);
        }

        public async Task StopAsync()
        {
            if (_hubConnection != null && IsConnected)
               await _hubConnection.StopAsync();
        }

        public async Task DeleteMessageAsync(MessageInformationDto message)
        {
            await _hubConnection.SendAsync("DeleteMessageAsync", message);
        }
    }
}
