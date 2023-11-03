using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using RChat.UI.Common.JwtTokenParser.Interfaces;
using System.Security.Claims;

namespace RChat.UI.Common.AuthenticationProvider
{
    public class ChatAuthenticationProvider : AuthenticationStateProvider
    {
        private ILocalStorageService _localStorageService;
        private IJwtTokenParser _jwtTokenParser;
        public ChatAuthenticationProvider(ILocalStorageService localStorageService,
                                          IJwtTokenParser jwtTokenParser)
        {
            _localStorageService = localStorageService;
            _jwtTokenParser = jwtTokenParser;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var jwtToken = await _localStorageService.GetItemAsync<string>("auth-jwt-token");

            var authState = string.IsNullOrWhiteSpace(jwtToken) ?
                 new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())) :
                 new AuthenticationState(new ClaimsPrincipal(
                     new ClaimsIdentity(_jwtTokenParser.ParseJwtToClaims(jwtToken), "JwtAuth"
                     )));

            NotifyAuthenticationStateChanged(Task.FromResult(authState));

            return authState;

        }
    }
}
