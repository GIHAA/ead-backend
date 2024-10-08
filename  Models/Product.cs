/*
 * File: Product.cs
 * Project: Healthy Bites._Models
 * Description: Represents the Product model in the database. This class includes properties such as VendorId, 
 *              ProductName, ProductDescription, CategoryId, Price, StockQuantity, and ProductStatus.
 *              The status of the product is represented by an enum (Active, Inactive, Promoted), and it 
 *              includes the URL for the product image.
 */




using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TechFixBackend._Models
{
    public enum ProductStatus
    {
        Active,
        Inactive,
        Promoted,
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
