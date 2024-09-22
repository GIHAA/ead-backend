using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace TechFixBackend.Hubs
{
    public class NotificationHub : Hub
    {
        // Called when a client connects
        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier; // Use your own logic to identify users, e.g., from claims
            if (userId != null)
            {
                // Add the user to their own group for targeted notifications
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            }
            await base.OnConnectedAsync();
        }

        // Called when a client disconnects
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context.UserIdentifier;
            if (userId != null)
            {
                // Remove the user from their group when they disconnect
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
