using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using RChat.Domain.Users.DTO;
using RChat.UI.Common.HttpClientPwa;
using RChat.UI.ViewModels;

namespace RChat.UI.Pages
{
    public partial class LoginComponent : ComponentBase
    {
        [Inject]
        public ILocalStorageService LocalStorageService { get; set; }
        public LoginViewModel ViewModel { get; set; } = new();

        public async void Submit()
        {
            var response = await new HttpClientPwa().
                SendPostRequestAsync<LoginViewModel,UserTokenResponse>(HttpClientPwa.LoginApiUrl, ViewModel);
            if (response.IsSuccessStatusCode && response.Result.IsSucceed)
                    LocalStorageService.SetItemAsync("authToken", response.Result.Token);               
        }
    }
}
