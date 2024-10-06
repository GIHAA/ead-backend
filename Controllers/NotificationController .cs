using Microsoft.AspNetCore.Mvc;
using TechFixBackend.Services;

namespace TechFixBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly NotificationService _notificationService;
        private readonly NotificationManager _notificationManager;

        public NotificationController(NotificationService notificationService, NotificationManager notificationManager)
        {
            _notificationService = notificationService;
            _notificationManager = notificationManager;
        }

        // Send a notification to all users
        [HttpPost("broadcast")]
        public async Task<IActionResult> BroadcastMessage([FromBody] string message)
        {
            await _notificationService.SendNotificationAsync(message);
            return Ok(new { Message = "Notification broadcasted successfully to all users" });
        }

        // Send a notification to a specific user by userId
        [HttpPost("sendToUser/{userId}")]
        public async Task<IActionResult> SendMessageToUser(string userId, [FromBody] string message)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is required");
            }

            var connections = _notificationManager.GetConnections(userId);
            if (!connections.Any())
            {
                return NotFound(new { Message = $"No active connections found for user {userId}" });
            }

            await _notificationService.SendNotificationToUserAsync(userId, message);
            return Ok(new { Message = $"Notification sent to user {userId}" });
        }


        // Send a notification to a specific group (e.g., admin, vendor)
        [HttpPost("sendToGroup")]
        public async Task<IActionResult> SendMessageToGroup([FromQuery] string groupName, [FromBody] string message)
        {
            if (string.IsNullOrEmpty(groupName))
            {
                return BadRequest("Group name is required");
            }

            await _notificationService.SendNotificationToGroupAsync(groupName, message);
            return Ok(new { Message = $"Notification sent to group {groupName}" });
        }

        // Manually add a connection to a group (can be useful for testing)
        [HttpPost("addToGroup")]
        public async Task<IActionResult> AddConnectionToGroup([FromQuery] string connectionId, [FromQuery] string groupName)
        {
            if (string.IsNullOrEmpty(connectionId) || string.IsNullOrEmpty(groupName))
            {
                return BadRequest("Connection ID and Group name are required");
            }

            await _notificationService.AddConnectionToGroup(connectionId, groupName);
            return Ok(new { Message = $"Connection {connectionId} added to group {groupName}" });
        }

        // Manually remove a connection from a group (can be useful for testing)
        [HttpPost("removeFromGroup")]
        public async Task<IActionResult> RemoveConnectionFromGroup([FromQuery] string connectionId, [FromQuery] string groupName)
        {
            if (string.IsNullOrEmpty(connectionId) || string.IsNullOrEmpty(groupName))
            {
                return BadRequest("Connection ID and Group name are required");
            }

            await _notificationService.RemoveConnectionFromGroup(connectionId, groupName);
            return Ok(new { Message = $"Connection {connectionId} removed from group {groupName}" });
        }
    }
}
