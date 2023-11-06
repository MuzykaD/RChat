﻿using RChat.UI.Common;
using RChat.UI.Common.HttpClientPwa.Interfaces;
using RChat.UI.Common.HttpClientPwa;
using RChat.UI.ViewModels;
using RChat.Domain.Users;
using BlazorBootstrap;
using RChat.Domain.Repsonses;

namespace RChat.UI.Services.UserService
{
    public class UserService : IUserService
    {
        private IHttpClientPwa _httpClientPwa;

        public UserService(IHttpClientPwa httpClientPwa)
        {
            _httpClientPwa = httpClientPwa;
        }

        public async Task<ApiRequestResult<GridListDto<UserInformationViewModel>>> GetUsersListAsync(int take, int skip = 0, string? searchValue = null)
        {
            string searchValueQuery = string.IsNullOrWhiteSpace(searchValue) ? string.Empty : $"&value={searchValue}";
            return await _httpClientPwa.SendGetRequestAsync<GridListDto<UserInformationViewModel>>(RChatApiRoutes.Users + $"?skip={skip}&take={take}{searchValueQuery}" );
        }

        public async Task<ApiRequestResult<IEnumerable<UserInformationViewModel>>> SearchUsersByValueAsync(string searchValue)
        {
            return await _httpClientPwa.SendGetRequestAsync<IEnumerable<UserInformationViewModel>>(RChatApiRoutes.SearchUsers + $"?value={searchValue}");
        }
    }
}