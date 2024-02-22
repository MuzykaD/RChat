using RChat.Domain.Repsonses;
using RChat.UI.Common;
using RChat.UI.Common.HttpClientPwa.Interfaces;

namespace RChat.UI.Services.JarvisService
{
    public class JarvisService : IJarvisService
    {
        private IHttpClientPwa _httpClientPwa;
        public JarvisService(IHttpClientPwa httpClientPwa)
        {
            _httpClientPwa = httpClientPwa; 
        }
        public async Task<string> SendMessageToJarvisAsync(string message)
        {
            var formattedMessage = message.Trim().Replace(' ', '+');
            var response = await _httpClientPwa.SendGetRequestAsync<ApiResponse>(RChatApiRoutes.Kernel + $"?prompt={formattedMessage}");
            return response.Result.Message!;
        }
    }
}
