using Microsoft.AspNetCore.Components;
using RChat.UI.Services.AccountService;
using RChat.UI.ViewModels.InformationViewModels;

namespace RChat.UI.Pages.Account
{
    public partial class PersonalPageComponent : ComponentBase
    {
        [Inject]
        public IAccountService UserService { get; set; }
        public UserInformationViewModel ViewModel { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            var apiResult = await UserService.GetPersonalInformationAsync();
            if (apiResult.IsSuccessStatusCode)
            {
                ViewModel = apiResult.Result!;
            }
        }
    }
}
