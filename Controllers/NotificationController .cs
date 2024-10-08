
/*
 * File: NotificationController.cs
 * Project: HealthyBites
 * Description: This file contains the NotificationController class, which provides API endpoints for managing notifications in the system.
 *              It allows for broadcasting notifications, sending notifications to specific users or groups, and managing notification statuses (read/unread).
 * 
 * Authors: Cooray N.T.L. it21177996
 * 
 * Classes:
 * - NotificationController: Handles HTTP requests for managing notifications in the system.
 * 
 * Methods:
 * - BroadcastMessage: Sends a notification to all connected clients.
 * - SendMessageToUser: Sends a notification to a specific user based on their userId.
 * - SendMessageToGroup: Sends a notification to a specific group.
 * - AddConnectionToGroup: Adds a client connection to a SignalR group.
 * - RemoveConnectionFromGroup: Removes a client connection from a SignalR group.
 * - GetUserNotifications: Retrieves all notifications for a specific user.
 * - MarkNotificationAsRead: Updates the status of a user's notifications (e.g., marking them as read).
 * 
 */


using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver;
using HealthyBites._Models;
using HealthyBites.Dtos;
using HealthyBites.Services;

namespace HealthyBites.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly NotificationService _notificationService;
        private readonly NotificationManager _notificationManager;
        private readonly MongoDBContext _mongoContext;
        

        public NotificationController(MongoDBContext mongoContext, NotificationService notificationService, NotificationManager notificationManager)
        {
            _mongoContext = mongoContext;
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

        [HttpGet("getUserNotifications/{userId}")]
        public async Task<IActionResult> GetUserNotifications(string userId)
        {
            
            var notifications = await _mongoContext.Notifications
                .Find(n => n.UserId == userId)
                .ToListAsync();

            
            var notificationDtos = new List<NotificationDto>();

            
            foreach (var notification in notifications)
            {
                
                var product = await _mongoContext.Products
                    .Find(p => p.Id == notification.ProductId)
                    .FirstOrDefaultAsync();

              
                var order = await _mongoContext.Orders
                    .Find(o => o.Id == notification.OrderId)
                    .FirstOrDefaultAsync();

               
                var notificationDto = new NotificationDto
                {
                    Notification = notification,
                    Products = product != null ? new List<Product> { product } : new List<Product>(),  // Add product if exists
                    Orders = order != null ? new List<Order> { order } : new List<Order>()             // Add order if exists
                };

               
                notificationDtos.Add(notificationDto);
            }

            
            return Ok(notificationDtos);
        }

        [HttpPut("markAsRead/{id}")]
        public async Task<IActionResult> MarkNotificationAsRead(string id, [FromBody] NotificationStatusUpdateRequest request)
        {
            // Filter to find notifications for the specific user
            var filter = Builders<Notification>.Filter.Eq(n => n.UserId, id) & Builders<Notification>.Filter.Eq(n => n.Status, request.OldStatus);

            // Set the status to the new status provided
            var update = Builders<Notification>.Update.Set(n => n.Status, request.NewStatus);

            // Update the notification
            var result = await _mongoContext.Notifications.UpdateManyAsync(filter, update);

            if (result.ModifiedCount > 0)
            {
                // Return the number of notifications updated
                return Ok(new { Message = $"{result.ModifiedCount} notifications updated to {request.NewStatus}" });
            }
            else
            {
                // If no notifications were found or no update was performed
                return NotFound(new { Message = "No notifications found or already in the desired status" });
            }
        }




    }
}
