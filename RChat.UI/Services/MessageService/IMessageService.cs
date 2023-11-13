using RChat.Domain.Messages.Dto;
using RChat.Domain.Repsonses;
using RChat.UI.Common;
using RChat.UI.Services.Common;
using RChat.UI.ViewModels.InformationViewModels;

namespace RChat.UI.Services.MessageService
{
    public interface IMessageService : IModelInformationList<MessageInformationViewModel>
    {
        Task SendMessageAsync(MessageInformationDto messageDto);
    }
}
