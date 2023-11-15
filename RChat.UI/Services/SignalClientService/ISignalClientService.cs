using RChat.Domain.Common;
using RChat.Domain.Messages.Dto;
using RChat.UI.ViewModels.InformationViewModels;

namespace RChat.UI.Services.SignalClientService
{
    public interface ISignalClientService
    {
        public event Action<MessageInformationDto> OnMessageReceived;
        public event Action<MessageInformationDto> OnMessageDelete;
        public Task StartAsync();
        public Task StopAsync();
        public Task CallSendMessageAsync(MessageInformationDto messageDto, NotificationArguments notificationArguments);
        public Task JoinChatGroupAsync(int chatId);
        public Task LeaveChatGroupAsync(int chatId);
        
        public Task DeleteMessageAsync(MessageInformationDto message);
        public  Task RegisterUserSignalGroupsAsync();
    }
}
