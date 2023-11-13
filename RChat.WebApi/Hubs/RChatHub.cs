using Microsoft.AspNetCore.SignalR;
using RChat.Domain.Messages.Dto;

namespace RChat.WebApi.Hubs
{
    public class RChatHub : Hub
    {
        public async Task SendMessageAsync(MessageInformationDto message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
        public async Task ChatNotificationAsync(string message, string receiverEmail, string senderEmail)
        {
            await Clients.All.SendAsync("ReceiveChatNotification", message, receiverEmail, senderEmail);
        }
    }
}
