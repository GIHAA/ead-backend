using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TechFixBackend._Models
{
    // Enum to represent the Category Status
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

        // Use enum for status and set default to Active
        [BsonElement("CategoryStatus")]
        [BsonRepresentation(BsonType.String)]
        public CategoryStatus CatStatus { get; set; } = CategoryStatus.Active;
    }
}
