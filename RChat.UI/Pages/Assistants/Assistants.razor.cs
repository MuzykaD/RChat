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
        protected RadzenUpload UploadComponent { get; set; }
        public List<Assistant> EntityList { get; set; }
        protected IBrowserFile? ChosenFile { get; set; }
        protected bool IsFileChosen => ChosenFile != null;

        protected override async Task OnInitializedAsync()
        {
            var assistants = await HttpClientPwa.SendGetRequestAsync<GridListDto<Assistant>>(RChatApiRoutes.Assistants);
            EntityList = assistants.Result.SelectedEntities.ToList();

        }
        /*
        protected async Task UploadFileActionAsync()
        {
            if (ChosenFile != null)
            {
                var fileContent = await ChosenFileToByteArrayAsync();
                await AssistantFileService.AddFileAsync(ChosenFile.Name, fileContent, "user");
                EntityList.Add(new() { FileName = ChosenFile.Name });
                ChosenFile = null;
                StateHasChanged();
            }

        }
        private async Task<byte[]> ChosenFileToByteArrayAsync()
        {
            var buffer = new byte[ChosenFile.Size];
            await using var stream = ChosenFile!.OpenReadStream();
            await stream.ReadAsync(buffer, 0, (int)ChosenFile.Size);
            return buffer;
        }
        protected async Task DeleteFileActionAsync(string id)
        {
            await AssistantFileService.RemoveFileAsync(id);
            EntityList = EntityList.Where(file => !file.Id.Equals(id)).ToList();
            StateHasChanged();
        }
        
        protected async Task OnFileUploadChange(InputFileChangeEventArgs args)
        {
            ChosenFile = args.File;
        }

       
       public void OnProgress(UploadProgressArgs args, string name)
        {
            Console.WriteLine($"{args.Progress}% '{name}' / {args.Loaded} of {args.Total} bytes.");

            if (args.Progress == 100)
            {
                foreach (var file in args.Files)
                {
                    Console.WriteLine($"Uploaded: {file.Name} / {file.Size} bytes");
                }
            }
        }
        */
    }
}
