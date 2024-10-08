/*
 * File: ProductCat.cs
 * Project: Healthy Bites
 * Description: This file defines the ProductCat model for the Healthy Bites system. It represents product categories, including details such as
 *              category name, description, image URL, and status. The data is stored in MongoDB, with BSON attributes used for proper serialization.
 * 
 * Authors: Kuruppu K.A.G.S.R it21165252
 * 
 * Classes:
 * - ProductCat: Represents a product category in the Healthy Bites system, including name, description, and status.
 * - CategoryStatus: Enum representing the status of a category (Active, Inactive).
 */

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HealthyBites._Models
{
    public enum CategoryStatus
    {
        Active,
        Inactive
    }

    public class ProductCat
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("CategoryName")]
        public string CatName { get; set; }

        [BsonElement("CategoryDescription")]
        public string CatDescription { get; set; }

        [BsonElement("CategoryImageUrl")]
        public string CatImageUrl { get; set; }

        [BsonElement("CategoryStatus")]
        [BsonRepresentation(BsonType.String)]
        public CategoryStatus CatStatus { get; set; } = CategoryStatus.Active;
    }
}
