
using Microsoft.AspNetCore.Components;
using Radzen;
using RChat.UI.Common.ComponentHelpers;
using RChat.UI.Services.BlazorAuthService;
using RChat.UI.ViewModels.AuthenticationViewModels;

namespace RChat.UI.Pages.Authentication
{
    public class RegisterComponent : ComponentBase, IFormComponentBase<RegisterViewModel>
    {
        [Inject]
        public IBlazorAuthService AuthService { get; set; }
        [Inject]
        NotificationService NotificationService { get; set; }
        [Inject]
        NavigationManager NavigationManager { get; set; }
        public bool ShowMessage { get; set; }
        public string Message { get; set; }
        protected bool _successfullyRegistered;
        public RegisterViewModel ViewModel { get; set; } = new();
        public async Task SubmitFormAsync()
        {
            var response = await AuthService.RegisterUserAsync(ViewModel);
            Message = response.Result.Message;
            
            NotificationService.Notify(new()
            {
                Severity = response.IsSuccessStatusCode ? NotificationSeverity.Info : NotificationSeverity.Error,
                Summary = Message,
                Duration = 4000,              
            });
            if (response.IsSuccessStatusCode)
                NavigationManager.NavigateTo("/login");
           
        }
    }
}
