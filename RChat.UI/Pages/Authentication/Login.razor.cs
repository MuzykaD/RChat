using BlazorBootstrap;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using RChat.Domain.Users.DTO;
using RChat.UI.Common.ComponentHelpers;
using RChat.UI.Common.HttpClientPwa;
using RChat.UI.Common.HttpClientPwa.Interfaces;
using RChat.UI.Services.BlazorAuthService;
using RChat.UI.ViewModels;

namespace RChat.UI.Pages.Authentication
{
    public partial class LoginFormComponent : ComponentBase, IFormComponentBase<LoginViewModel>
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        [Inject]
        private IBlazorAuthService AuthService { get; set; }
        public LoginViewModel ViewModel { get; set; } = new();
        protected bool ShowMessage { get; set; }
        protected string Message { get; set; }
        public async Task SubmitFormAsync()
        {
            var response = await AuthService.LoginUserAsync(ViewModel);
            if (response.IsSuccessStatusCode && response.Result.IsSucceed)
                NavigationManager.NavigateTo("/");

            else
            {
                Message = response.Message;
                ShowMessage = true;
            }
        }

    }
}
