/*
 * File: Notification.cs
 * Project: HealthyBites
 * Description: This file defines the models and methods related to notifications in the HealthyBites system.
 *              It handles the structure of notification data stored in the MongoDB database, including details such as message, status (read/unread),
 *              product, order ID, and timestamps. It also includes the functionality for updating notification statuses.
 * 
 * Authors: Cooray N.T.L. it21177996 | Perera W.H.T.H. it21165498
 * 
 * Classes:
 * - Notification: Represents a notification sent to users regarding different events such as order updates or messages.
 * - NotificationStatusUpdateRequest: Represents the structure of a request to update the status of a notification.
 * 
 */


using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HealthyBites._Models
{
    public class Notification
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("UserId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }

        [BsonElement("Message")]
        public string Message { get; set; }

        [BsonElement("Status")]
        public string Status { get; set; } = "unread";

        [BsonIgnoreIfNull]
        [BsonElement("ProductId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? ProductId { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("OrderId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? OrderId { get; set; }   

        [BsonElement("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
