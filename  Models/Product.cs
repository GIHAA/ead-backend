using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TechFixBackend._Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("VendorId")]
        [BsonRepresentation(BsonType.ObjectId)] // This will reference the Vendor's _id field
        public string VendorId { get; set; }

        [BsonElement("ProductName")] 
        public string ProductName { get; set; }

        [BsonElement("ProductDescription")] 
        public string ProductDescription { get; set; }

        [BsonElement("Category")] 
        public string Category { get; set; }

        [BsonElement("Price")] 
        public double Price { get; set; }

        [BsonElement("StockQuantity")]
        public int StockQuantity { get; set; }

        [BsonElement("ProductStatus")] 
        public string ProductStatus { get; set; }

        [BsonElement("ProductImageUrl")] 
        public string ProductImageUrl { get; set; }
    }
}
