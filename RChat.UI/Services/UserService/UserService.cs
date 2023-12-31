﻿using RChat.UI.Common;
using RChat.UI.Common.HttpClientPwa.Interfaces;
using RChat.UI.Common.HttpClientPwa;
using RChat.Domain.Users;
using BlazorBootstrap;
using RChat.Domain.Repsonses;
using RChat.UI.ViewModels.InformationViewModels;

namespace RChat.UI.Services.UserService
{
    public class UserService : IUserService
    {
        private IHttpClientPwa _httpClientPwa;

        public UserService(IHttpClientPwa httpClientPwa)
        {
            _httpClientPwa = httpClientPwa;
        }

        public async Task<ApiRequestResult<GridListDto<UserInformationViewModel>>> GetInformationListAsync(int page, int size, string? value = null, string? orderBy = null, string? orderByType = null)
        {
            return await _httpClientPwa
                .SendGetRequestAsync<GridListDto<UserInformationViewModel>>
                (
                RChatApiRoutes.Users +
                HttpQueryBuilder.BuildGridListQuery(page, size, value!, orderBy, orderByType)
                );
        }
    }
}
