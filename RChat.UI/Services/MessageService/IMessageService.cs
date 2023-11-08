using RChat.Domain.Repsonses;
using RChat.UI.Common;
using RChat.UI.ViewModels;

namespace RChat.UI.Services.MessageService
{
    public interface IMessageService
    {
        Task<ApiRequestResult<GridListDto<MessageInformationViewModel>>> GetMessagesListAsync(int page, int size, string? value = null, string? orderBy = null, string? orderByType = null);
    }
}
