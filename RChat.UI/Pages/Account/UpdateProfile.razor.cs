using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Radzen;
using RChat.UI.Common.ComponentHelpers;
using RChat.UI.Services.AccountService;
using RChat.UI.ViewModels.InformationViewModels;

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
        [Inject]
        public NotificationService NotificationService { get; set; }
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
                NotificationService.Notify(new()
                {
                    Summary = "Profile updated!",
                    Duration = 4000,
                    Severity = NotificationSeverity.Success
                });

            }
            else
            {
                Message = result.Result.Message;
                NotificationService.Notify(new()
                {
                    Summary = "Error! It seems such email is taken already!",
                    Duration = 4000,
                    Severity = NotificationSeverity.Error
                });
            }

        }

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
