using RChat.Domain.Messages.Dto;
using RChat.Domain.Repsonses;
using RChat.UI.Common;
using RChat.UI.Common.HttpClientPwa.Interfaces;
using RChat.UI.ViewModels.InformationViewModels;
using RChat.UI.ViewModels.Message;

namespace RChat.UI.Services.MessageService
{
    public class MessageService : IMessageService
    {
        private IHttpClientPwa _httpClientPwa;

        public MessageService(IHttpClientPwa httpClientPwa)
        {
            _httpClientPwa = httpClientPwa;
        }

        public async Task DeleteMessageByIdAsync(int messageId)
        {
            await _httpClientPwa.SendDeleteRequestAsync(RChatApiRoutes.Messages + $"?messageId={messageId}");
        }

        public async Task<ApiRequestResult<GridListDto<MessageInformationViewModel>>> GetInformationListAsync(int page, int size, string? value = null, string? orderBy = null, string? orderByType = null)
        {
            return await _httpClientPwa
              .SendGetRequestAsync<GridListDto<MessageInformationViewModel>>
              (
              RChatApiRoutes.Messages +
              HttpQueryBuilder.BuildGridListQuery(page, size, value!, orderBy, orderByType)
              );
        }
        //change id returning logic
        public async Task<int> SendMessageAsync(MessageInformationDto messageDto)
        {           
          var result =  await _httpClientPwa.SendPostRequestAsync<MessageInformationDto, ApiResponse>(RChatApiRoutes.Messages, messageDto);
          return int.Parse(result.Result.Message);
        }
    }
}
