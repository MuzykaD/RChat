using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
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
        [Inject]
        protected NavigationManager NavigationManager { get; set; }
        [Inject]
        protected IJSRuntime Js { get; set; }
        [CascadingParameter]
        protected ISignalClientService SignalClientService { get; set; }
        [Parameter]
        [SupplyParameterFromQuery]
        public int? UserId { get; set; }
        // todo
        private string _currentUserEmail;
        private int _currentUserId;
        //
        protected ChatViewModel ChatViewModel { get; set; }
        protected bool InitComplete { get; set; }

        protected string? MessageValue { get; set; }

        protected async override Task OnInitializedAsync()
        {
            var apiResponse = await ChatService.GetPrivateChatByUserIdAsync(UserId.Value);
            ChatViewModel = apiResponse.Result!;
            InitComplete = true;
            //todo
            var state = await StateProvider.GetAuthenticationStateAsync();
            _currentUserEmail = state.User.FindFirstValue(ClaimTypes.Email);
            _currentUserId = int.Parse(state.User.FindFirstValue(ClaimTypes.NameIdentifier));
            //
            NavigationManager.LocationChanged += async (sender, arg) => await LocationChanged(sender, arg);
            SignalClientService.OnMessageReceived -= OnMessageReceived;
            SignalClientService.OnMessageReceived += OnMessageReceived;
            await SignalClientService.JoinChatGroupAsync(ChatViewModel.Id);
           
        }
        protected async Task SendMessageAsync()
        {
            if (!string.IsNullOrWhiteSpace(MessageValue))
            {
                var message = new MessageInformationDto()
                { SenderId = _currentUserId, SenderEmail = _currentUserEmail, ChatId = ChatViewModel.Id, Content = MessageValue, SentAt = DateTime.Now };
                await MessageService.SendMessageAsync(message);
                await SignalClientService.CallSendMessageAsync(UserId.Value, message);
                MessageValue = string.Empty;
            }
        }
        private async void OnMessageReceived(MessageInformationDto messageViewModel)
        {
            ChatViewModel!.Messages!.Add(messageViewModel);
            StateHasChanged();
          
        }


        async Task LocationChanged(object sender, LocationChangedEventArgs e)
        {
            await SignalClientService.LeaveChatGroupAsync(ChatViewModel.Id);
        }
    }
}
