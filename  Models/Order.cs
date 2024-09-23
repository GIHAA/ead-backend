using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace TechFixBackend._Models
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string CustomerId { get; set; }

        public List<OrderItem> Items { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public string Status { get; set; } = "Processing";

        public float TotalAmount { get; set; }

        public string DeliveryAddress { get; set; }

        public DateTime? DispatchedDate { get; set; }
    }

    public class OrderItem
    {
        [BsonElement("ProductId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ProductId { get; set; }

        public int Quantity { get; set; }

        public float Price { get; set; }

        public string Status { get; set; } = "Processing";

        public float TotalPrice => Quantity * Price;
    }
}
