using BlazorApp.Hubs;
using BlazorApp.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace BlazorApp.Services
{
    public class NotifierService : IAsyncDisposable
    {
        public List<string> Messages { get; private set; } = new List<string>();
        private HubConnection hubConnection;

        public async Task ConfigureHub(HubConnection connection)
        {
            hubConnection = connection;

            await hubConnection.StartAsync();
        }

        public async Task Send(string message) =>
            await hubConnection.SendAsync("SendMessage", message, message);

        public bool IsConnected =>
            hubConnection.State == HubConnectionState.Connected;

        public async ValueTask DisposeAsync()
        {
            if (hubConnection is not null)
            {
                await hubConnection.DisposeAsync();
            }
        }
    }
}
