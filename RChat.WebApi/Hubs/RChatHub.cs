using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using RChat.Domain.Common;
using RChat.Domain.Messages.Dto;

namespace RChat.WebApi.Hubs
{
    
    public class RChatHub : Hub
    {
        [Authorize]
        public async Task SendMessageAsync(MessageInformationDto message, NotificationArguments notificationArguments)
        {
            string groupName = $"in-chat-{message.ChatId}";
            await Clients.Group(groupName).SendAsync("ReceiveMessage", message);
            await ChatNotificationAsync(message.ChatId, notificationArguments);
        }

        public async Task ChatNotificationAsync(int chatId, NotificationArguments notificationArguments)
        {
            string groupName = $"out-chat-{chatId}";
            await Clients.OthersInGroup(groupName).SendAsync("ReceiveNotification", notificationArguments);
        }

        public async Task EnterChatGroupAsync(int chatId)
        {
            string groupName = $"-chat-{chatId}";
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "out"+groupName);
            await Groups.AddToGroupAsync(Context.ConnectionId, "in"+groupName);
        }

        public async Task LeaveChatGroupAsync(int chatId)
        {
            string groupName = $"-chat-{chatId}";
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "in" + groupName);
            await Groups.AddToGroupAsync(Context.ConnectionId, "out" + groupName);
        }

    }
}
