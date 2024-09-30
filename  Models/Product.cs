using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TechFixBackend._Models
{
    public enum ProductStatus
    {
        Active,
        Inactive
    }
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("VendorId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string VendorId { get; set; }

        [BsonElement("ProductName")]
        public string ProductName { get; set; }

        [BsonElement("ProductDescription")]
        public string ProductDescription { get; set; }

        [BsonElement("CategoryId")]
        public string CategoryId { get; set; }

        [BsonElement("Price")]
        public double Price { get; set; }

        [BsonElement("StockQuantity")]
        public int StockQuantity { get; set; } = 0;

        [BsonElement("ProductStatus")]
        [BsonRepresentation(BsonType.String)]
        public ProductStatus ProductStatus { get; set; } = ProductStatus.Active;

        [BsonElement("ProductImageUrl")]
        public string ProductImageUrl { get; set; }
    }
}
