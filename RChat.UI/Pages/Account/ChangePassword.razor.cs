using Microsoft.AspNetCore.Components;
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
        protected bool ShowMessage { get; set; }
        protected string Message { get; set; }
        public async Task SubmitFormAsync()
        {
            var response = await UserService.ChangeUserPasswordAsync(ViewModel);

            Message = response.Result.Message!;
            ShowMessage = true;
        }
    }
}
