using Microsoft.AspNetCore.Components;
using RChat.UI.Services.UserService;
using RChat.UI.ViewModels;

namespace RChat.UI.Pages.Users
{
    public partial class PersonalPageComponent : ComponentBase
    {
        [Inject]
        public IUserService UserService { get; set; }
        public PersonalPageViewModel ViewModel { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            var apiResult = await UserService.GetPersonalInformationAsync();
            ViewModel = apiResult.Result!;
        }
    }
}
