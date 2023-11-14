using RChat.Domain.Chats.Dto;
using RChat.Domain.Messages.Dto;
using RChat.Domain.Repsonses;
using RChat.UI.Common;
using RChat.UI.Common.HttpClientPwa.Interfaces;
using RChat.UI.ViewModels.Chat;
using RChat.UI.ViewModels.InformationViewModels;

namespace RChat.UI.Services.ChatService
{
    public class ChatService : IChatService
    {
        private IHttpClientPwa _httpClientPwa;

        public ChatService(IHttpClientPwa httpClientPwa)
        {
            _httpClientPwa = httpClientPwa;
        }

        public async Task CreatePublicGroupAsync(HashSet<UserInformationViewModel> users, string groupName)
        {
            var membersId = users.Select(u => u.Id).ToList();
            var grid = new CreateGroupChatDto() { MembersId = membersId, GroupName = groupName };
            await _httpClientPwa.SendPostRequestAsync<CreateGroupChatDto, ApiResponse>(RChatApiRoutes.ChatsGroup, grid);
        }

        public async Task<ApiRequestResult<GridListDto<ChatInformationViewModel>>> GetInformationListAsync(int page, int size, string? value = null, string? orderBy = null, string? orderByType = null)
        {
            return await _httpClientPwa
              .SendGetRequestAsync<GridListDto<ChatInformationViewModel>>
              (
              RChatApiRoutes.Chats +
              HttpQueryBuilder.BuildGridListQuery(page, size, value!, orderBy, orderByType)
              );
        }

        public async Task<ApiRequestResult<ChatViewModel>> GetPrivateChatByUserIdAsync(int userId)
        {
            return await _httpClientPwa
             .SendGetRequestAsync<ChatViewModel>
             (
             RChatApiRoutes.ChatsPrivate + $"/{userId}"
             );
        }
    }
}
