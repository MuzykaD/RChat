using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using RChat.Domain.Common;
using RChat.Domain.Messages;
using RChat.Domain.Messages.Dto;
using RChat.UI.Common.ComponentHelpers.ChatWindowBase;
using RChat.UI.Services.ChatService;
using RChat.UI.Services.MessageService;
using RChat.UI.Services.SignalClientService;
using RChat.UI.ViewModels.Chat;
using System.Security.Claims;

namespace RChat.UI.Pages.Chats
{
    public partial class GroupChatWindowComponent : ComponentBase, IChatWindowBase
    {
        [Parameter]
        [SupplyParameterFromQuery]
        public int GroupId { get; set; }
        [Inject]
        public IChatService ChatService { get; set; }
        [Inject]
        public IMessageService MessageService { get; set; }
        [Inject]
        public AuthenticationStateProvider StateProvider { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public ISignalClientService SignalClientService { get; set; }
        public ChatViewModel ChatViewModel { get; set; }
        public bool InitComplete { get; set; }
        public string? MessageValue { get; set; }
        public bool ShowUsersList { get; set; }

        // todo
        private string _currentUserEmail;
        protected int _currentUserId;


        protected async override Task OnInitializedAsync()
        {
            var apiResponse = await ChatService.GetGroupChatByIdAsync(GroupId);
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

            SignalClientService.OnMessageDelete -= OnMessageDeleted;
            SignalClientService.OnMessageDelete += OnMessageDeleted;
            await SignalClientService.JoinChatGroupAsync(ChatViewModel.Id);
        }

        public async Task SendMessageAsync()
        {
            if (!string.IsNullOrWhiteSpace(MessageValue))
            {
                var message = new MessageInformationDto()
                { SenderId = _currentUserId, SenderEmail = _currentUserEmail, ChatId = ChatViewModel.Id, Content = MessageValue, SentAt = DateTime.Now };
                var messageId = await MessageService.SendMessageAsync(message);
                message.Id = messageId;
                var notificationArguments = new NotificationArguments()
                {
                    ReferenceId = ChatViewModel.Id,
                    ChatName = ChatViewModel.Name,
                    IsGroup = ChatViewModel.IsGroupChat
                };
                await SignalClientService.CallSendMessageAsync(message, notificationArguments);
                MessageValue = string.Empty;
            }
        }

        public void OnMessageReceived(MessageInformationDto messageViewModel)
        {
            ChatViewModel!.Messages!.Add(messageViewModel);
            StateHasChanged();
        }
        protected async Task LocationChanged(object sender, LocationChangedEventArgs e)
        {
            await SignalClientService.LeaveChatGroupAsync(ChatViewModel.Id);
        }
        protected async Task DeleteMessageAsync(int messageId)
        {
            var message = ChatViewModel.Messages.FirstOrDefault(x => x.Id == messageId);
            ChatViewModel.Messages.Remove(message);
            await MessageService.DeleteMessageByIdAsync(messageId);
            await SignalClientService.DeleteMessageAsync(message);
        }

        protected void OnMessageDeleted(MessageInformationDto message) 
        {
            ChatViewModel.Messages = ChatViewModel.Messages.Where(c => c.Id != message.Id).ToList();
            StateHasChanged();
        }


    }
}
