using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


public class Order
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string CustomerId { get; set; }

    public DateTime OrderDate { get; set; }

    public string OrderStatus { get; set; }

    public float TotalAmount { get; set; }

    public string DeliveryAddress { get; set; }

    public string DeliveryStatus { get; set; }
}
