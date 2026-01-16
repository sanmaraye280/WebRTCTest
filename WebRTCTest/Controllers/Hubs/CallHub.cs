using Microsoft.AspNetCore.SignalR;

namespace WebRTCTest.Controllers.Hubs;

public class CallHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        await Clients.Caller.SendAsync("YourConnectionId", Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public async Task SendOffer(string targetConnectionId, string offer)
    {
        await Clients.Client(targetConnectionId)
            .SendAsync("ReceiveOffer", Context.ConnectionId, offer);
    }

    public async Task SendAnswer(string targetConnectionId, string answer)
    {
        await Clients.Client(targetConnectionId)
            .SendAsync("ReceiveAnswer", Context.ConnectionId, answer);
    }

    public async Task SendIceCandidate(string targetConnectionId, string candidate)
    {
        await Clients.Client(targetConnectionId)
            .SendAsync("ReceiveIceCandidate", Context.ConnectionId, candidate);
    }
}
