using RChat.Domain.Messages.Dto;
using RChat.UI.ViewModels.InformationViewModels;

namespace RChat.UI.Services.SignalClientService
{
    public interface ISignalClientService
    {
        public event Action<MessageInformationDto> OnMessageReceived;
        public Task StartAsync(bool forceStartRequired = false);
        public Task StopAsync();
        public Task CallSendMessageAsync(int recipientId, MessageInformationDto messageDto);
        public Task JoinChatGroupAsync(int chatId);
        public Task LeaveChatGroupAsync(int chatId);
    }
}
