using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace JobPortalSystem.Models
{
    public class NotificationHub : Hub
    {
        // Track User Connections
        private static readonly ConcurrentDictionary<string, string> UserConnections = new ConcurrentDictionary<string, string>();

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.Identity?.Name; // Assuming you're using user authentication
            if (userId != null)
            {
                UserConnections[userId] = Context.ConnectionId;
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User?.Identity?.Name;
            if (userId != null)
            {
                UserConnections.TryRemove(userId, out _);
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendApplicationStatusUpdate(string userId, string message)
        {
            if (UserConnections.TryGetValue(userId, out var connectionId))
            {
                await Clients.Client(connectionId).SendAsync("ReceiveApplicationStatusUpdate", message);
            }
            else
            {
                Console.WriteLine("User not connected.");
            }
        }


        public async Task SendMessage(string user, string message)
        {
            Console.WriteLine($"Sending message to user: {user} - {message}");
            await Clients.User(user).SendAsync("ReceiveApplicationStatusUpdate", message);
        }
    }
}
