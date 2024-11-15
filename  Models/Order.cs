/*
 * File: Order.cs
 * Project: Healthy Bites
 * Description: This file defines the models related to orders in the HealthyBites system. It includes the Order, OrderItem, and Cancellation classes,
 *              which represent the structure of order-related data stored in the MongoDB database. Each class contains relevant properties 
 *              to handle the order lifecycle, including order creation, item details, and cancellation requests.
 * 
 */

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HealthyBites._Models
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


        public string DeliveryStatus { get; set; }


        public DateTime? DispatchedDate { get; set; }

        public Cancellation? Cancellation { get; set; }
    }

    public class OrderItem
    {
        [BsonElement("ProductId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ProductId { get; set; }


        public string VendorId { get; set; }


        public int Quantity { get; set; }


        public float Price { get; set; }


        public string Status { get; set; } = "Processing";


        public float TotalPrice => Quantity * Price;


    }


    public class Cancellation
    {
        public bool Requested { get; set; } = false;

        public string Status { get; set; } = "none";

        public string Reason { get; set; }

        public DateTime? RequestedAt { get; set; }

        public DateTime? ResolvedAt { get; set; }
    }
}
