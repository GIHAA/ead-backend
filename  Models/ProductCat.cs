using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TechFixBackend._Models
{
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
    }
}
