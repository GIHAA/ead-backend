using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TechFixBackend._Models
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
