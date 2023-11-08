
using Microsoft.AspNetCore.Components;
using Radzen;
using RChat.UI.Common.ComponentHelpers;
using RChat.UI.Services.BlazorAuthService;
using RChat.UI.ViewModels;

namespace RChat.UI.Pages.Authentication
{
    public class RegisterComponent : ComponentBase, IFormComponentBase<RegisterViewModel>
    {
        [Inject]
        public IBlazorAuthService AuthService { get; set; }
        [Inject]
        NotificationService NotificationService { get; set; }
        public bool ShowMessage { get; set; }
        public string Message { get; set; }
        protected bool _successfullyRegistered;
        public RegisterViewModel ViewModel { get; set; } = new();
        public async Task SubmitFormAsync()
        {
            var response = await AuthService.RegisterUserAsync(ViewModel);
            Message = response.Result.Message;
            ShowMessage = true;
            _successfullyRegistered = response.IsSuccessStatusCode;

            NotificationService.Notify(new()
            {
                Severity = NotificationSeverity.Info,
                Summary = Message
            });
        }
    }
}
