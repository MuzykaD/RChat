using BlazorBootstrap;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Radzen;
using RChat.Domain.Users.DTO;
using RChat.UI.Common.ComponentHelpers;
using RChat.UI.Common.HttpClientPwa;
using RChat.UI.Common.HttpClientPwa.Interfaces;
using RChat.UI.Services.BlazorAuthService;
using RChat.UI.ViewModels.AuthenticationViewModels;

namespace RChat.UI.Pages.Authentication
{
    public partial class LoginFormComponent : ComponentBase, IFormComponentBase<LoginViewModel>
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }
        [Inject]
        private IBlazorAuthService AuthService { get; set; }
        [Inject]
        public NotificationService NotificationService { get; set; }
        public LoginViewModel ViewModel { get; set; } = new();
        protected bool ShowMessage { get; set; }
        protected string Message { get; set; }
        public async Task SubmitFormAsync()
        {
            var response = await AuthService.LoginUserAsync(ViewModel);
            if (response.IsSuccessStatusCode && response.Result.IsSucceed)
            {
                NotificationService.Notify(new() { Severity = NotificationSeverity.Success, Summary = @"Welcome!", Duration = 3000 });
                NavigationManager.NavigateTo("/");
            }

            else
            {
                Message = response.Message;
                ShowMessage = true;
                NotificationService.Notify(new() {
                    Severity = NotificationSeverity.Error, Summary = $"{Message}!", Duration = 3000});
            }
        }

    }
}
