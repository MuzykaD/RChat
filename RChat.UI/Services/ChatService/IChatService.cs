using RChat.Domain.Repsonses;
using RChat.UI.Common;
using RChat.UI.ViewModels;

namespace RChat.UI.Services.ChatService
{
    public interface IChatService
    {
        Task<ApiRequestResult<GridListDto<ChatInformationViewModel>>> GetChatsListAsync(int page, int size, string? value = null, string? orderBy = null, string? orderByType = null);
    }
}
