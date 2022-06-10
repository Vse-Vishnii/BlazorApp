using BlazorApp.Data;
using BlazorApp.Hubs;
using BlazorApp.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Services
{
    public class NotifierService
    {
        private readonly ApplicationDbContext db;
        private readonly IServiceScopeFactory scopeFactory;
        public List<string> Messages { get; } = new List<string>();
        public HubConnection HubConnection { get; private set; }

        public NotifierService(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
            using (var scope = scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                db.Notifications.RemoveRange(db.Notifications);
                db.SaveChanges();
            }
        }

        public async Task ConfigureHub(HubConnection connection, Func<string, Task> action)
        {
            HubConnection = connection;

            HubConnection.On<string>("ReceiveMessage", async message =>
                await action(message)
            );

            await HubConnection.StartAsync();
        }

        public async Task Send(string message)
        {
            await HubConnection.SendAsync("SendMessage", message);
            Console.WriteLine($"-----------------------Send Message: {message}-------------------------------");
        }

        public bool IsConnected =>
            HubConnection.State == HubConnectionState.Connected;

        public async Task AddMessage(string message)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                db.Add(new Notification { Text = message });
                await db.SaveChangesAsync();
            }
        }

        public async Task<List<string>> GetMessages()
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                return await db.Set<Notification>().Select(notification => notification.Text).ToListAsync();
            }
        }
    }
}
