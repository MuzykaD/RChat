using HigLabo.OpenAI;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Radzen;
using Radzen.Blazor;
using RChat.Application.Contracts.Assistant;
using RChat.Domain.Repsonses;
using RChat.Domain.Assistants;
using RChat.UI.Common;
using RChat.UI.Common.ComponentHelpers;
using RChat.UI.Common.HttpClientPwa;
using RChat.UI.Common.HttpClientPwa.Interfaces;
using System;
using System.Data.SqlClient;
using System.Net.Http.Headers;

namespace RChat.UI.Pages.Assistants
{
    public class AssistantComponent : ComponentBase
    {        
        [Inject]
        protected IAssistantFileService AssistantFileService { get; set; }
        [Inject]
        protected IHttpClientPwa HttpClientPwa { get; set; }
        [Inject]
        protected NavigationManager NavigationManager { get; set; }
        protected RadzenUpload UploadComponent { get; set; }
        public List<Assistant> EntityList { get; set; }
        protected IBrowserFile? ChosenFile { get; set; }
        protected bool IsFileChosen => ChosenFile != null;

        protected override async Task OnInitializedAsync()
        {
            var assistants = await HttpClientPwa.SendGetRequestAsync<GridListDto<Assistant>>(RChatApiRoutes.Assistants);
            EntityList = assistants.Result.SelectedEntities.ToList();

        }
        protected void NavigateToAssistantFiles(string assistantId)
        {
            NavigationManager.NavigateTo($"/assistant-files/{assistantId}");
        }
    }
}
