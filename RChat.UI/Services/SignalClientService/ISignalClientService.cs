using RChat.Domain.Messages.Dto;
using RChat.UI.ViewModels.InformationViewModels;

namespace RChat.UI.Services.SignalClientService
{
    public interface ISignalClientService
    {
        public event Action<MessageInformationDto> OnMessageReceived;
        public Task StartAsync();
        public Task CallSendMessageAsync(string recipientEmail, MessageInformationDto messageDto);
        public Task JoinChatGroupAsync(int chatId);
    }
}
