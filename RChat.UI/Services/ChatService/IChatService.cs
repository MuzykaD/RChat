using RChat.Domain.Repsonses;
using RChat.UI.Common;
using RChat.UI.Services.Common;
using RChat.UI.ViewModels.Chat;
using RChat.UI.ViewModels.InformationViewModels;

namespace RChat.UI.Services.ChatService
{
    public interface IChatService : IModelInformationList<ChatInformationViewModel>
    {
        public Task<ApiRequestResult<ChatViewModel>> GetPrivateChatByUserIdAsync(int userId);
    }
}
