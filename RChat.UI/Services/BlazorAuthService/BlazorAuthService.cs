using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using RChat.Domain.Repsonses;
using RChat.Domain.Users.DTO;
using RChat.UI.Common;
using RChat.UI.Common.HttpClientPwa;
using RChat.UI.Common.HttpClientPwa.Interfaces;
using RChat.UI.ViewModels;

namespace RChat.UI.Services.BlazorAuthService
{
    internal class BlazorAuthService : IBlazorAuthService
    {
        private IHttpClientPwa _httpClientPwa;
        private ILocalStorageService _localStorageService;

        public BlazorAuthService(IHttpClientPwa httpClientPwa, ILocalStorageService localStorageService)
        {
            _httpClientPwa = httpClientPwa;
            _localStorageService = localStorageService;
        }

        public async Task<ApiRequestResult<UserTokenResponse>> LoginUserAsync(LoginViewModel registerViewModel)
        {
            var response = await _httpClientPwa.
                SendPostRequestAsync<LoginViewModel, UserTokenResponse>(HttpClientPwa.LoginApiUrl, registerViewModel);
            if (response.IsSuccessStatusCode && response.Result.IsSucceed)
                await _localStorageService.SetItemAsync("auth-jwt-token", response.Result.Token);
            return response;

        }

        public async Task LogoutUserAsync()
        {
            await _localStorageService.RemoveItemAsync("auth-jwt-token");
        }

        public async Task<ApiRequestResult<ApiResponse>> RegisterUserAsync(RegisterViewModel registerViewModel)
        {
            return await _httpClientPwa.
                  SendPostRequestAsync<RegisterViewModel, ApiResponse>(HttpClientPwa.RegisterApiUrl, registerViewModel);
        }
    }
}
