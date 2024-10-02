
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class CartItem
{
    [BsonElement("ProductId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ProductId { get; set; }

    [BsonElement("Quantity")]
    public int Quantity { get; set; } = 1;

    [BsonElement("Price")]
    public double Price { get; set; }
}