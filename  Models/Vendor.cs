using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TechFixBackend._Models
{
    public class Vendor
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } // Primary identifier typically used in MongoDB
        
        [BsonElement("vendorName")] // Specifies the name of the field in MongoDB
        public string VendorName { get; set; }

        [BsonElement("averageRating")]
        public float AverageRating { get; set; }

        [BsonElement("comments")]
        public string Comments { get; set; }

        [BsonElement("isActive")]
        public bool IsActive { get; set; }

        // If VendorId is meant to be a separate field, ensure it's not used as a conflicting identifier.
        [BsonElement("VendorId")] // Adjusted the field mapping to avoid conflict
        [BsonRepresentation(BsonType.ObjectId)]
        public string VendorId { get; set; } 
    }
}
