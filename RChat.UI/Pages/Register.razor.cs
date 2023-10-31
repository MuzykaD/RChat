using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using RChat.Domain.Repsonses;
using RChat.Domain.Users.DTO;
using RChat.UI.Common.HttpClientPwa;
using RChat.UI.ViewModels;

namespace RChat.UI.Pages
{
    public class RegisterComponent : ComponentBase
    {
        public RegisterViewModel ViewModel { get; set; } = new();

        public async void Submit()
        {
            var response = await new HttpClientPwa().
                SendPostRequestAsync<RegisterViewModel, ApiResponse>(HttpClientPwa.RegisterApiUrl, ViewModel);
            
                Console.WriteLine(response.Result.Message);
        }
    }
}
