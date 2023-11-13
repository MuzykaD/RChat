using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using RChat.Domain.Messages.Dto;
using RChat.UI.Services.ChatService;
using RChat.UI.Services.MessageService;
using RChat.UI.Services.SignalClientService;
using RChat.UI.ViewModels.Chat;
using RChat.UI.ViewModels.InformationViewModels;
using System.Security.Claims;

namespace RChat.UI.Pages.Chats
{
    public partial class ChatWindowComponent : ComponentBase
    {
        [Inject]
        protected IChatService ChatService { get; set; }
        [Inject]
        protected IMessageService MessageService { get; set; }
        [Inject]
        protected AuthenticationStateProvider StateProvider { get; set; }
        [CascadingParameter]
        protected ISignalClientService SignalClientService { get; set; }
        [Parameter]
        [SupplyParameterFromQuery]
        public string Email { get; set; }
        private string _currentUserEmail;
        protected ChatViewModel ChatViewModel { get; set; }
        protected bool InitComplete { get; set; }

        protected string? MessageValue { get; set; }

        protected async override Task OnInitializedAsync()
        {
            var apiResponse = await ChatService.GetPrivateChatByEmail(Email);
            ChatViewModel = apiResponse.Result!;
            InitComplete = true;
            var state = await StateProvider.GetAuthenticationStateAsync();
            _currentUserEmail = state.User.FindFirstValue(ClaimTypes.Email);
            SignalClientService.OnMessageReceived -= OnMessageReceived;
            SignalClientService.OnMessageReceived += OnMessageReceived;
           await SignalClientService.JoinChatGroupAsync(ChatViewModel.Id);
        }

        private void OnMessageReceived(MessageInformationDto messageViewModel)
        {
            ChatViewModel.Messages.Add(messageViewModel);
            StateHasChanged();
        }

        protected async Task SendMessageAsync()
        {
            if(!string.IsNullOrWhiteSpace(MessageValue)) 
            {
                var message = new MessageInformationDto()
                { SenderEmail = _currentUserEmail,ChatId = ChatViewModel.Id, Content = MessageValue, SentAt = DateTime.Now };
                await MessageService.SendMessageAsync(message);
                await SignalClientService.CallSendMessageAsync(Email, message);
                MessageValue = string.Empty;
            }          
        }
    }
}
