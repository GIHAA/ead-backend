/*
 * File: Product.cs
 * Project: Healthy Bites
 * Description: This file defines the Product model for the Healthy Bites system. It includes the Product and ProductStatus enum, representing
 *              the various states of a product in the system. The Product class contains properties related to product details, such as name,
 *              description, price, stock quantity, and status. This model is designed to store product information in MongoDB.
 * 
 * Authors: Kuruppu K.A.G.S.R it21165252
 * 
 * Classes:
 * - Product: Represents a product in the Healthy Bites system, including vendor details, price, stock, and status.
 * - ProductStatus: Enum representing different states of a product (Active, Inactive, Promoted).
 */

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HealthyBites._Models
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
