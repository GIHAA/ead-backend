using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;


public class Order
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string CustomerId { get; set; }

    public List<OrderItem> Items { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    public string Status { get; set; } = "Processing";

    public float TotalAmount { get; set; }

    public string DeliveryAddress { get; set; }

    public string DeliveryStatus { get; set; }
    public DateTime? DispatchedDate { get; set; }
}

public class OrderItem
{
    public string ProductId { get; set; }
    public int Quantity { get; set; }
    public float Price { get; set; }

    // Total price for this item (Quantity * Price)
    public float TotalPrice => Quantity * Price;
}