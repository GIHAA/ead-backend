using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Order
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string UserId { get; set; }
    
    public string VendorId { get; set; }
    
    public string Status { get; set; }
    
    public string PaymentMethod { get; set; }
    
    public string PaymentStatus { get; set; }
    
    public string DeliveryMethod { get; set; }
    
    public string DeliveryStatus { get; set; }
    
    public string DeliveryAddress { get; set; }
    
    public string DeliveryPhoneNumber { get; set; }
    
    public string DeliveryNote { get; set; }
    
    public List<OrderItem> Items { get; set; }
    
    public float TotalPrice { get; set; }
    
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    
    public DateTime? DeliveryDate { get; set; }
}

