using Microsoft.AspNetCore.SignalR;
using RChat.Domain.Messages.Dto;

namespace RChat.WebApi.Hubs
{
    public class RChatHub : Hub
    {
        public async Task SendMessageAsync(int recipientId, MessageInformationDto message)
        {
            string groupName = $"in-chat-{message.ChatId}";
            await Clients.Group(groupName).SendAsync("ReceiveMessage", message);
            await ChatNotificationAsync(message);
        }
        public async Task ChatNotificationAsync(MessageInformationDto message)
        {
            string groupName = $"out-chat-{message.ChatId}";
            await Clients.OthersInGroup(groupName).SendAsync("ReceiveNotification", message);
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
