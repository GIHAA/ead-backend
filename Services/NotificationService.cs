using Microsoft.AspNetCore.SignalR;
using TechFixBackend.Hubs;

namespace TechFixBackend.Services
{
    public class NotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        // Send notification to all connected clients
        public async Task SendNotificationAsync(string message)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", message);
        }

        // Send a notification to a specific user by their userId
        public async Task SendNotificationToUserAsync(string userId, string message)
        {
            await _hubContext.Clients.Group(userId).SendAsync("ReceiveNotification", message);
        }

        // Send a notification to a specific group (e.g., admins, vendors)
        public async Task SendNotificationToGroupAsync(string groupName, string message)
        {
            await _hubContext.Clients.Group(groupName).SendAsync("ReceiveNotification", message);
        }

        // Add a connection to a group for targeted notifications
        public async Task AddConnectionToGroup(string connectionId, string groupName)
        {
            await _hubContext.Groups.AddToGroupAsync(connectionId, groupName);
        }

        // Remove a connection from a group
        public async Task RemoveConnectionFromGroup(string connectionId, string groupName)
        {
            await _hubContext.Groups.RemoveFromGroupAsync(connectionId, groupName);
        }
    }
}
