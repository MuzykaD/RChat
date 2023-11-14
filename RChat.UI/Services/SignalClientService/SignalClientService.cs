using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Radzen;
using RChat.Domain.Messages.Dto;
using RChat.UI.ViewModels.InformationViewModels;

namespace RChat.UI.Services.SignalClientService
{
    //Dispose
    public class SignalClientService : ISignalClientService
    {
        private HubConnection? _hubConnection;
        public event Action<MessageInformationDto> OnMessageReceived;
        private NotificationService _notificationService;
        private NavigationManager _navigationManager;
        public SignalClientService(NavigationManager navigationManager,NotificationService notificationService)
        {
            _navigationManager = navigationManager;
            _notificationService = notificationService;
        }
        private bool IsConnected => _hubConnection.State == HubConnectionState.Connected;
        public async Task CallSendMessageAsync(int recipientId, MessageInformationDto messageDto)
        {
            if (_hubConnection == null || !IsConnected)
                return;
            await _hubConnection.SendAsync("SendMessageAsync", recipientId, messageDto);
        }

        public async Task StartAsync()
        {
            if (_hubConnection != null && IsConnected)
                return;
            _hubConnection = new HubConnectionBuilder().WithUrl("https://localhost:7089/rChatHub").Build();

            _hubConnection.On<MessageInformationDto>("ReceiveMessage", message => OnMessageReceived?.Invoke(message));
            _hubConnection.On<MessageInformationDto>("ReceiveNotification", (message) =>
            {
                _notificationService.Notify(new()
                {
                    Summary = $"{message.SenderEmail} is trying to reach you!",
                    Severity = NotificationSeverity.Info,
                    Duration = 2000,
                    Click = (notification) => _navigationManager.NavigateTo($"/chats/private?userId={message.SenderId}")
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
    }
}
