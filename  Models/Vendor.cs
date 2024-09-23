using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TechFixBackend._Models
{
    public class Vendor
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } 

        [BsonElement("vendorName")] // Specifies the name of the field in MongoDB
        public string VendorName { get; set; } 

        [BsonElement("averageRating")]
        public float AverageRating { get; set; } 

        [BsonElement("comments")]
        public string Comments { get; set; }

        [BsonElement("isActive")]
        public bool IsActive { get; set; } 

        [BsonElement("userId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public required string VendorId { get; set; } 
    }
}
