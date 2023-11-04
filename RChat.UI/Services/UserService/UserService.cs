using RChat.UI.Common;
using RChat.UI.Common.HttpClientPwa.Interfaces;
using RChat.UI.Common.HttpClientPwa;
using RChat.UI.ViewModels;

namespace RChat.UI.Services.UserService
{
    public class UserService : IUserService
    {
        private IHttpClientPwa _httpClientPwa;

        public UserService(IHttpClientPwa httpClientPwa)
        {
            _httpClientPwa = httpClientPwa;
        }
        public async Task<ApiRequestResult<IEnumerable<UserInformationViewModel>>> GetUsersListAsync()
        {
            return await _httpClientPwa.SendGetRequestAsync<IEnumerable<UserInformationViewModel>>(HttpClientPwa.Users);
        }
    }
}
