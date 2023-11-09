using Microsoft.AspNetCore.Components;
using Radzen;
using RChat.UI.Common.ComponentHelpers;
using RChat.UI.Services.AccountService;
using RChat.UI.ViewModels;

namespace RChat.UI.Pages.Account
{
    public partial class ChangePasswordFormComponent : ComponentBase, IFormComponentBase<ChangePasswordViewModel>
    {
        [Inject]
        private IAccountService UserService { get; set; }
        public ChangePasswordViewModel ViewModel { get; set; } = new();     
        [Inject]
        NavigationManager NavigationManager { get; set; }
        [Inject]
        public NotificationService NotificationService { get; set; }
        public async Task SubmitFormAsync()
        {
            var response = await UserService.ChangeUserPasswordAsync(ViewModel);
            NotificationService.Notify(new()
            {
                Summary = response.Result.Message,
                Duration = 4000,
                Severity = response.IsSuccessStatusCode ? NotificationSeverity.Success : NotificationSeverity.Error
            });

            if (response.IsSuccessStatusCode)
                NavigationManager.NavigateTo("/profile");
            
        }
    }
}
