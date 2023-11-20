using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Channels;

namespace RChat.WebApi.Hubs
{
    [Authorize]
    public class RVideoHub : Hub
    {
        public async Task Join(string channel)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, channel);
            await Clients.OthersInGroup(channel).SendAsync("Join", Context.ConnectionId);
        }
        public async Task Leave(string channel)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, channel);
            await Clients.OthersInGroup(channel).SendAsync("Leave", Context.ConnectionId);
        }

        public async Task SignalWebRtc(string channel, string type, string payload)
        {
            await Clients.OthersInGroup(channel).SendAsync("SignalWebRtc", channel, type, payload);
        }

        public async Task RegisterMultipleGroupsAsync(IEnumerable<int> groupsId)
        {
            var tasks = new List<Task>();
            string groupName;
            foreach (int id in groupsId)
            {
                groupName = $"video-{id}";
                tasks.Add(Groups.AddToGroupAsync(Context.ConnectionId, groupName));
            }
            await Task.WhenAll(tasks);
        }

        public async Task AskForConfirmation(string channel, int chatId)
        {
            await Clients.OthersInGroup(channel).SendAsync("AskForConfirmation", channel, chatId);
        }

        public async Task ConfirmationResponse(string channel, bool isConfirmed)
        {
            await Clients.OthersInGroup(channel).SendAsync("ConfirmationResult", isConfirmed);
        }
    }
}
