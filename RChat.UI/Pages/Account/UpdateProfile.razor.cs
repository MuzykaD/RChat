using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using RChat.UI.Common.ComponentHelpers;
using RChat.UI.Services.AccountService;
using RChat.UI.ViewModels;

namespace RChat.UI.Pages.Account
{
    public class UpdateProfileComponent : ComponentBase, IFormComponentBase<UserInformationViewModel>
    {
        [Inject]
        public IAccountService UserService { get; set; }
        [Inject]
        public ILocalStorageService LocalStorageService { get; set; }
        [Inject]
        NavigationManager NavigationManager { get; set; }
        public bool ShowMessage { get; set; }
        public string Message { get; set; }
        public UserInformationViewModel ViewModel { get; set; } = new();

        public async Task SubmitFormAsync()
        {
            var result = await UserService.UpdatePersonalInformationAsync(ViewModel);
            if (result.IsSuccessStatusCode)
            { 
                await LocalStorageService.SetItemAsync<string>("auth-jwt-token", result.Result.Token);
                NavigationManager.NavigateTo("/profile");
            }
            else
            {
                Message = result.Result.Message;
                ShowMessage = true;
            }

        }

        protected override async Task OnInitializedAsync()
        {
            var apiResult = await UserService.GetPersonalInformationAsync();
            ViewModel = apiResult.Result!;
        }
    }
}
