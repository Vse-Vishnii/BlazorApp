using BlazorApp.Data;
using BlazorApp.Hubs;
using BlazorApp.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace BlazorApp.Services
{
    public class NotifierService
    {
        private readonly MongoContext mongoContext;

        public HubConnection HubConnection { get; private set; }

        public NotifierService(MongoContext mongoContext)
        {
            this.mongoContext = mongoContext;
        }

        public async Task ConfigureHub(HubConnection hubConnection,Func<string, Task> action)
        {
            HubConnection = hubConnection;

            HubConnection.On<string>("ReceiveMessage", async message =>
                {
                    await action(message);
                }
                
            );

            await HubConnection.StartAsync();
        }

        public async Task Send(string message)
        {
            await HubConnection.SendAsync("SendMessage", message);
            await AddMessage(message);
        }

        public bool IsConnected =>
            HubConnection.State == HubConnectionState.Connected;

        public async Task AddMessage(string message)
        {
            mongoContext.Create(new Notification(message, DateTime.Now));
        }

        public async Task<List<string>> GetMessages()
        {
            var notifications = await mongoContext.GetNotifications();
            return notifications.Select(notification =>notification.Text).ToList();
        }
    }
}
