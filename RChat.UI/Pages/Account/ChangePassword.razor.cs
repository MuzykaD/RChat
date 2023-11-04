using Microsoft.AspNetCore.Components;
using RChat.UI.Common.ComponentHelpers;
using RChat.UI.Services.UserService;
using RChat.UI.ViewModels;

namespace RChat.UI.Pages.Users
{
    public partial class ChangePasswordFormComponent : ComponentBase, IFormComponentBase<ChangePasswordViewModel>
    {
        [Inject]
        private IUserService UserService { get; set; }
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
