using Microsoft.AspNetCore.SignalR;
using RChat.Domain.Messages.Dto;

namespace RChat.WebApi.Hubs
{
    public class RChatHub : Hub
    {
        public async Task SendMessageAsync(string recipientEmail, MessageInformationDto message)
        {
            string groupName = $"chat-{message.ChatId}";
            await Clients.Group(groupName).SendAsync("ReceiveMessage", message);
            await ChatNotificationAsync(recipientEmail, message);
        }
        public async Task ChatNotificationAsync(string recipientEmail, MessageInformationDto message)
        {
            string groupName = $"chat-{message.ChatId}";
            await Clients.OthersInGroup(groupName).SendAsync("ReceiveNotification", message.SenderEmail);
        }

        public async Task JoinChatGroupAsync(int chatId)
        {
            string groupName = $"chat-{chatId}";
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

    }
}
