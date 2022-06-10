using Microsoft.AspNetCore.SignalR;

namespace BlazorApp.Hubs
{
    public class ApplicationHub : Hub
    {
        public const string HubUrl = "/app";
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
