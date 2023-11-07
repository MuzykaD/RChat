using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using RChat.UI.Common.HttpClientPwa.Interfaces;
using RChat.UI.Common.JwtTokenParser.Interfaces;
using System.Security.Claims;

namespace RChat.UI.Common.AuthenticationProvider
{
    public class ChatAuthenticationProvider : AuthenticationStateProvider
    {
        private ILocalStorageService _localStorageService;
        private IJwtTokenParser _jwtTokenParser;
        private IHttpClientPwa _httpClientPwa;
        public ChatAuthenticationProvider(ILocalStorageService localStorageService,
                                          IJwtTokenParser jwtTokenParser,
                                          IHttpClientPwa httpClientPwa)
        {
            _localStorageService = localStorageService;
            _jwtTokenParser = jwtTokenParser;
            _httpClientPwa = httpClientPwa;
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

        public void NotifyUserAuthentication(string token)
        {
            _httpClientPwa.TryAddJwtToken(token);
            var claims = _jwtTokenParser.ParseJwtToClaims(token);
            var identity = new ClaimsIdentity(claims, "jwtAuthType");
            var authState = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
            NotifyAuthenticationStateChanged(authState);
        }

        public void NotifyUserLogout()
        {
            _httpClientPwa.TryDeleteJwtToken();
            var authState = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
            NotifyAuthenticationStateChanged(authState);
        }
    }
}
