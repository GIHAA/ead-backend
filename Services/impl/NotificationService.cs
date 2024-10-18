
/*
 * File: NotificationService.cs
 * Project: HealthyBites
 * Description: This file contains the implementation of the NotificationService class, which is responsible for sending and managing notifications in the system.
 *              It provides methods to send notifications to individual users, groups, or all clients. It also integrates with SignalR to manage user connections
 *              and supports sending notifications with additional details related to products and orders.
 * 
 * Authors: Cooray N.T.L. it21177996 
 * 
 * Classes:
 * - NotificationService: Handles sending notifications to users, managing SignalR groups, and storing notifications in MongoDB.
 * 
 * Methods:
 * - SendNotificationAsync: Sends a notification to all connected clients.
 * - SendNotificationToUserAsync: Sends a notification to a specific user.
 * - SendNotificationToGroupAsync: Sends a notification to a specific group of users.
 * - SendNotificationWithDetailsAsync: Sends a notification to a user with product and order details.
 * 
 */


using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver;
using HealthyBites._Models;
using HealthyBites.Hubs;

namespace HealthyBites.Services
{
    public class NotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IMongoCollection<Notification> _notifications;
        private readonly IMongoCollection<Product> _products;
        private readonly IMongoCollection<Order> _orders;

        public NotificationService(IHubContext<NotificationHub> hubContext, MongoDBContext dbContext)
        {
            _hubContext = hubContext;
            _notifications = dbContext.Notifications;
            _products = dbContext.Products;
            _orders = dbContext.Orders;
        }
        public async Task StoreNotificationAsync(Notification notification)
        {
            await _notifications.InsertOneAsync(notification);
        }

        // Send notification to all connected clients
        public async Task SendNotificationAsync(string message)
        {
            // Create and store the notification in the database
            var notification = new Notification
            {
                Message = message,
                CreatedAt = DateTime.UtcNow,
                Status = "unread"
            };
            await StoreNotificationAsync(notification);

            await _hubContext.Clients.All.SendAsync("ReceiveNotification", message);
        }

        // Send a notification to a specific user by their userId
        public async Task SendNotificationToUserAsync(string userId, string message)
        {

            var notification = new Notification
            {
                UserId = userId,
                Message = message,
                CreatedAt = DateTime.UtcNow,
                Status = "unread"
            };
            await StoreNotificationAsync(notification);

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

        // Method for special cases with product and order information
        public async Task SendNotificationWithDetailsAsync(string userId, string message, string? productId, string? orderId)
        {
            // Only fetch product status if ProductId is provided
            string? productStatus = null;
            if (!string.IsNullOrEmpty(productId))
            {
                var product = await _products.Find(p => p.Id == productId).FirstOrDefaultAsync();
                if (product != null)
                {
                    productStatus = product.ProductStatus.ToString();
                }
            }

            // Only fetch order status if OrderId is provided
            string? orderStatus = null;
            if (!string.IsNullOrEmpty(orderId))
            {
                var order = await _orders.Find(o => o.Id == orderId).FirstOrDefaultAsync();
                if (order != null)
                {
                    orderStatus = order.Status;
                }
            }

            // Create and store the notification
            var notification = new Notification
            {
                UserId = userId,
                Message = message,
                ProductId = !string.IsNullOrEmpty(productId) ? productId : null,  // Only set ProductId if it's not null
                OrderId = !string.IsNullOrEmpty(orderId) ? orderId : null,        // Only set OrderId if it's not null
                CreatedAt = DateTime.UtcNow,
                Status = "unread"
            };

            await StoreNotificationAsync(notification);

            // Send the notification with the message and statuses
            await _hubContext.Clients.Group(userId).SendAsync("ReceiveNotification", new
            {
                Message = message,
                ProductStatus = productStatus,
                OrderStatus = orderStatus
            });
        }

        // Add this method to the NotificationService class
        public async Task SendNotificationToAdminAsync(string message)
        {
            var notification = new Notification
            {
                Message = message,
                CreatedAt = DateTime.UtcNow,
                Status = "unread"
            };
            await StoreNotificationAsync(notification);

            await _hubContext.Clients.Group("Admins").SendAsync("ReceiveAdminNotification", message);
        }


    }
}
