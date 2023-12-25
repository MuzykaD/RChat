using Microsoft.AspNetCore.Components;
using RChat.Application.Contracts.Assistant;
using RChat.Domain.Assistants;
using RChat.Domain.Assistants.Dto;
using RChat.UI.Common;
using RChat.UI.Common.HttpClientPwa.Interfaces;

namespace RChat.UI.Pages.Assistants
{
    public class AssistantInfoComponent : ComponentBase
    {
        [Inject]
        public IHttpClientPwa HttpClientPwa { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Parameter]
        public string AssistantId { get; set; }
        protected AssistantInfoDto ViewModel { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            var apiResult = await HttpClientPwa.SendGetRequestAsync<AssistantInfoDto>(RChatApiRoutes.Assistants + $"?assistantId={AssistantId}");
            ViewModel = apiResult.Result;
        }

        protected void NavigateToAssistantFiles()
        {
            NavigationManager.NavigateTo($"/assistant-files/{AssistantId}");
        }
        protected void NavigateToAssistantUpdate()
        {
            NavigationManager.NavigateTo($"/assistant-update/{AssistantId}");
        }
    }
}
