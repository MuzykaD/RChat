using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using RChat.Domain.Repsonses;
using RChat.Domain.Users.DTO;
using RChat.UI.Common;
using RChat.UI.Common.AuthenticationProvider;
using RChat.UI.Common.HttpClientPwa;
using RChat.UI.Common.HttpClientPwa.Interfaces;
using RChat.UI.ViewModels.AuthenticationViewModels;

namespace RChat.UI.Services.BlazorAuthService
{
    internal class BlazorAuthService : IBlazorAuthService
    {
        private IHttpClientPwa _httpClientPwa;
        private ILocalStorageService _localStorageService;
        private readonly AuthenticationStateProvider _authStateProvider;

        public BlazorAuthService(IHttpClientPwa httpClientPwa, ILocalStorageService localStorageService, AuthenticationStateProvider provider)
        {
            _httpClientPwa = httpClientPwa;
            _localStorageService = localStorageService;
            _authStateProvider = provider;  
        }

        public async Task<ApiRequestResult<UserTokenResponse>> LoginUserAsync(LoginViewModel registerViewModel)
        {
            var response = await _httpClientPwa.
                SendPostRequestAsync<LoginViewModel, UserTokenResponse>(RChatApiRoutes.Login, registerViewModel);
            if (response.IsSuccessStatusCode && response.Result.IsSucceed)
            { 
                await _localStorageService.SetItemAsync("auth-jwt-token", response.Result.Token);
                ((ChatAuthenticationProvider)_authStateProvider).NotifyUserAuthentication(response.Result.Token);
            }

            return response;

        }

        public async Task LogoutUserAsync()
        {

            await _localStorageService.RemoveItemAsync("auth-jwt-token");
            ((ChatAuthenticationProvider)_authStateProvider).NotifyUserLogout();
        }

        public async Task<ApiRequestResult<ApiResponse>> RegisterUserAsync(RegisterViewModel registerViewModel)
        {
            return await _httpClientPwa.
                  SendPostRequestAsync<RegisterViewModel, ApiResponse>(RChatApiRoutes.Register, registerViewModel);
        }
    }
}
