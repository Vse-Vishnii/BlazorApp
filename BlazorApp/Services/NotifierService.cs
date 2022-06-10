using BlazorApp.Hubs;
using BlazorApp.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace BlazorApp.Services
{
    public class NotifierService : IAsyncDisposable
    {
        public List<string> Messages { get; private set; } = new List<string>();
        public HubConnection HubConnection { get; private set; }

        public async Task ConfigureHub(HubConnection connection, Func<string, Task> action)
        {
            HubConnection = connection;

            HubConnection.On<string, string>("ReceiveMessage", async (message, message1) =>
                await action(message)
            );

            await HubConnection.StartAsync();
        }

        public async Task Send(string message) =>
            await HubConnection.SendAsync("SendMessage", message, message);

        public bool IsConnected =>
            HubConnection.State == HubConnectionState.Connected;

        public async ValueTask DisposeAsync()
        {
            if (HubConnection is not null)
            {
                await HubConnection.DisposeAsync();
            }
        }

        public async Task AddMessage(string message)
        {
            Messages.Add(message);
        }
    }
}
