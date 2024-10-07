using Microsoft.AspNetCore.SignalR;
using TechFixBackend.Services;

namespace TechFixBackend.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly NotificationManager _notificationManager;
        private readonly NotificationService _notificationService;

        // Inject the NotificationManager and NotificationService
        public NotificationHub(NotificationManager notificationManager, NotificationService notificationService)
        {
            _notificationManager = notificationManager;
            _notificationService = notificationService;
        }

        // Called when a client connects
        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier; 
            var connectionId = Context.ConnectionId;

            if (userId == null)
            {
                Console.WriteLine("UserIdentifier is null. Ensure proper authentication is set.");
            }
            else
            {
                Console.WriteLine($"User {userId} connected with connectionId {connectionId}.");
                _notificationManager.AddConnection(userId, connectionId);
                await _notificationService.AddConnectionToGroup(connectionId, userId);
            }

            await base.OnConnectedAsync();
        }


        // Called when a client disconnects
        public override async Task OnDisconnectedAsync(System.Exception exception)
        {
            var connectionId = Context.ConnectionId;
            var userId = _notificationManager.GetUserByConnectionId(connectionId);

            if (userId != null)
            {
                Console.WriteLine($"User {userId} with connectionId {connectionId} is disconnected.");
                _notificationManager.RemoveConnection(connectionId);
                await _notificationService.RemoveConnectionFromGroup(connectionId, userId);
            }

            await base.OnDisconnectedAsync(exception);
        }


        // Method to send a message to a specific group (e.g., admins, vendors)
        public async Task SendMessageToGroup(string groupName, string message)
        {
            await _notificationService.SendNotificationToGroupAsync(groupName, message);
        }

        public async Task SendNotification(string userId, string message)
        {
            // Add logic for sending notification to the user
            await Clients.User(userId).SendAsync("ReceiveNotification", message);
        }
    }
}
