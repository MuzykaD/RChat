using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;
using RChat.Domain.Messages.Dto;
using RChat.UI.Services.ChatService;
using RChat.UI.Services.MessageService;
using RChat.UI.Services.SignalClientService;
using RChat.UI.ViewModels.Chat;
using System.Security.Claims;

namespace RChat.UI.Common.ComponentHelpers.ChatWindowBase
{
    public interface IChatWindowBase
    {
        [Inject]
        IChatService ChatService { get; set; }
        [Inject]
        IMessageService MessageService { get; set; }
        [Inject]
        AuthenticationStateProvider StateProvider { get; set; }
        [Inject]
        NavigationManager NavigationManager { get; set; }
        [CascadingParameter]
        ISignalClientService SignalClientService { get; set; }

        public ChatViewModel ChatViewModel { get; set; }
        bool InitComplete { get; set; }
        string? MessageValue { get; set; }

        Task SendMessageAsync();
        void OnMessageReceived(MessageInformationDto messageViewModel);
    }
}
