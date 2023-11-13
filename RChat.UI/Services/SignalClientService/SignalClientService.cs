using Microsoft.AspNetCore.SignalR.Client;
using RChat.Domain.Messages.Dto;
using RChat.UI.ViewModels.InformationViewModels;

namespace RChat.UI.Services.SignalClientService
{
    //Dispose
    public class SignalClientService : ISignalClientService
    {
        private HubConnection? _hubConnection;
        public event Action<MessageInformationDto> OnMessageReceived;

        private bool IsConnected => _hubConnection.State == HubConnectionState.Connected;
        public async Task CallSendMessageAsync(string email, MessageInformationDto messageDto)
        {
            if (_hubConnection == null || !IsConnected)
                return;
            await _hubConnection.SendAsync("SendMessageAsync", messageDto);
        }

        public async Task StartAsync()
        {
            if (_hubConnection != null && IsConnected)
                return;
            _hubConnection = new HubConnectionBuilder().WithUrl("https://localhost:7089/rChatHub").Build();

            _hubConnection.On<MessageInformationDto>("ReceiveMessage", message => OnMessageReceived?.Invoke(message));
            await _hubConnection.StartAsync();
        }
    }
}
