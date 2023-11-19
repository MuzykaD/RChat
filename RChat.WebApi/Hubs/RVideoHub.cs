using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace RChat.WebApi.Hubs
{
    //[Authorize]
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

        // Used in rtc.razor/webrtcservice.cs
        public async Task SignalWebRtc(string channel, string type, string payload)
        {
            await Clients.OthersInGroup(channel).SendAsync("SignalWebRtc", channel, type, payload);
        }
    }
}
