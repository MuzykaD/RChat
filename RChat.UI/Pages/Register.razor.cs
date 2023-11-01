using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using RChat.Domain.Repsonses;
using RChat.Domain.Users.DTO;
using RChat.UI.Common.HttpClientPwa;
using RChat.UI.Common.HttpClientPwa.Interfaces;
using RChat.UI.Services.BlazorAuthService;
using RChat.UI.ViewModels;

namespace RChat.UI.Pages
{
    public class RegisterComponent : ComponentBase
    {
        public RegisterViewModel ViewModel { get; set; } = new();
        public bool ShowMessage { get; set; }
        public string Message { get; set; }
        [Inject]
        public IBlazorAuthService AuthService { get; set; }
        public async void Submit()
        {
            var response = await AuthService.RegisterUserAsync(ViewModel);

            if (response.IsSuccessStatusCode)
            {
                Message = response.Result.Message;
                ShowMessage = true;
            }
        }
    }
}
