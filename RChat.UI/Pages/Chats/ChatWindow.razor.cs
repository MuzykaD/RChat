using Microsoft.AspNetCore.Components;
using RChat.UI.Services.ChatService;
using RChat.UI.Services.MessageService;
using RChat.UI.ViewModels.Chat;

namespace RChat.UI.Pages.Chats
{
    public partial class ChatWindowComponent : ComponentBase
    {
        [Inject]
        public IChatService ChatService { get; set; }
        [Inject]
        public IMessageService MessageService { get; set; }
        [Parameter]
        [SupplyParameterFromQuery]
        public string Email { get; set; }
        protected ChatViewModel ChatViewModel { get; set; }
        protected bool InitComplete { get; set; }

        protected string? MessageValue { get; set; }

        protected async override Task OnInitializedAsync()
        {
            var apiResponse = await ChatService.GetPrivateChatByEmail(Email);
            ChatViewModel = apiResponse.Result!;
            InitComplete = true;
        }

        protected async Task SendMessageAsync()
        {
            if(!string.IsNullOrWhiteSpace(MessageValue)) 
            {
                await MessageService.SendMessageAsync(ChatViewModel.Id, MessageValue);
            }          
        }
    }
}
