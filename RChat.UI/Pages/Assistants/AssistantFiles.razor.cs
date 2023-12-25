using HigLabo.OpenAI;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;
using RChat.Application.Contracts.Assistant;
using RChat.Domain.AssistantFiles;
using RChat.Domain.Repsonses;
using RChat.UI.Common;
using RChat.UI.Common.ComponentHelpers;
using RChat.UI.Common.HttpClientPwa;
using RChat.UI.Common.HttpClientPwa.Interfaces;

namespace RChat.UI.Pages.Assistants
{
    public class AssistantFilesComponent : ComponentBase
    {
        [Parameter]
        public string AssistantId { get; set; }
        [Inject]
        public IAssistantFileService AssistantFileService { get; set; }
        [Inject]
        public IHttpClientPwa HttpClientPwa { get; set; }
        public List<AssistantFile> EntityList { get; set; } = new List<AssistantFile>();
        public int Count { get; set; }
        public int Size { get; set; }
        protected IBrowserFile? ChosenFile { get; set; }
        protected bool IsFileChosen => ChosenFile != null;

        protected async override Task OnInitializedAsync()
        {
            await AssistantFileService.InitializeAsync(AssistantId);
            var apiResponse = await HttpClientPwa.SendGetRequestAsync<GridListDto<AssistantFile>>(RChatApiRoutes.AssistantFiles + $"?assistantId={AssistantId}");
            EntityList = apiResponse.Result.SelectedEntities.ToList();
        }

        
        protected async Task UploadFileActionAsync()
        {
            if (ChosenFile != null)
            {
                var fileContent = await ChosenFileToByteArrayAsync();
                var fileAddResult = await AssistantFileService.AddFileAsync(ChosenFile.Name, fileContent, "assistants");
                var file = new CreateAssistantFileDto() { Name = ChosenFile.Name, Id = fileAddResult.Id, AssistantId = AssistantId, CreatedAt = DateTime.Now };
                await HttpClientPwa.SendPostRequestAsync<CreateAssistantFileDto, ApiRequestResult<ApiResponse>>(RChatApiRoutes.AssistantFile, file);
                await AssistantFileService.AttachFileToAssistantAsync(AssistantId, file.Id);
                // Investigate issue
                EntityList.Add(new() { Name = file.Name, CreatedAt = file.CreatedAt } );
                EntityList = new List<AssistantFile>(EntityList);
                //
                ChosenFile = null;
                StateHasChanged();
            }
            StateHasChanged();
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
            await HttpClientPwa.SendDeleteRequestAsync(RChatApiRoutes.AssistantFile + $"?fileId={id}");
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
             
    }
}
