using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using RChat.Domain.Common;
using RChat.Domain.Messages;
using RChat.Domain.Messages.Dto;

namespace RChat.WebApi.Hubs
{
    [Authorize]
    public class RChatHub : Hub
    {        
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
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "out" + groupName);
            await Groups.AddToGroupAsync(Context.ConnectionId, "in" + groupName);
        }

        public async Task LeaveChatGroupAsync(int chatId)
        {
            string groupName = $"-chat-{chatId}";
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "in" + groupName);
            await Groups.AddToGroupAsync(Context.ConnectionId, "out" + groupName);
        }

        public async Task DeleteMessageAsync(MessageInformationDto message)
        {
            string groupName = $"in-chat-{message.ChatId}";
            await Clients.OthersInGroup(groupName).SendAsync("OnMessageDelete", message);
        }

        public async Task UpdateMessageAsync(MessageInformationDto message)
        {
            string groupName = $"in-chat-{message.ChatId}";
            await Clients.OthersInGroup(groupName).SendAsync("OnMessageUpdate", message);
        }

        public async Task RegisterMultipleGroupsAsync(IEnumerable<int> groupsId)
        {
            var tasks = new List<Task>();
            string groupName;
            foreach (int id in groupsId)
            {
                groupName = $"out-chat-{id}";
                tasks.Add(Groups.AddToGroupAsync(Context.ConnectionId, groupName));
            }
            await Task.WhenAll(tasks);
        }      

    }
}
