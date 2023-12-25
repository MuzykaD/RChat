using Microsoft.AspNetCore.Components;
using RChat.Domain.Assistants;
using RChat.Domain.Assistants.Dto;
using RChat.UI.Common;
using RChat.UI.Common.HttpClientPwa.Interfaces;

namespace RChat.UI.Pages.Assistants
{
    public class AssistantInfoUpdateComponent : ComponentBase
    {
        [Inject]
        public IHttpClientPwa HttpClientPwa { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Parameter]
        public string AssistantId { get; set; }
        public AssistantUpdateDto ViewModel { get; set; } = new AssistantUpdateDto();

        protected async override Task OnInitializedAsync()
        {
            var apiResult = await HttpClientPwa.SendGetRequestAsync<AssistantInfoDto>(RChatApiRoutes.Assistants + $"?assistantId={AssistantId}");
            ViewModel = new()
            {
                Name = apiResult.Result.Name,
                Instructions = apiResult.Result.Context
            };
        }

        protected async Task SubmitFormAsync()
        {
            var url = RChatApiRoutes.Assistants + $"?assistantId={AssistantId}";
            await HttpClientPwa.SendPutRequestAsync<AssistantUpdateDto,ApiRequestResult<bool>>(url, ViewModel);
            NavigationManager.NavigateTo($"/assistant-info/{AssistantId}");
        }

        protected void NavigateBack()
        {
            NavigationManager.NavigateTo($"/assistant-info/{AssistantId}");
        }
    }
}
