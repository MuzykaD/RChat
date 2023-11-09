using RChat.Domain.Repsonses;
using RChat.UI.Common;
using RChat.UI.Common.HttpClientPwa.Interfaces;
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

        public async Task<ApiRequestResult<GridListDto<ChatInformationViewModel>>> GetInformationListAsync(int page, int size, string? value = null, string? orderBy = null, string? orderByType = null)
        {
            return await _httpClientPwa
              .SendGetRequestAsync<GridListDto<ChatInformationViewModel>>
              (
              RChatApiRoutes.Chats +
              HttpQueryBuilder.BuildGridListQuery(page, size, value!, orderBy, orderByType)
              );
        }
    }
}
