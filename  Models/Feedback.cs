/*
 * File: Feedback.cs
 * Project: Healthy Bites._Models
 * Description: Defines the Feedback model with relevant fields including vendor ID, customer ID, product ID, rating, and comments. 
 *              Each feedback is associated with a vendor, customer, and product, along with a timestamp for when it was created.
 * Author: Perera W.H.T.H. it21165498
 * 
 */


using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TechFixBackend._Models
{
    public class Feedback
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("vendorId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string VendorId { get; set; } // ObjectId of the Vendor (User with a vendor role)

        [BsonElement("customerId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CustomerId { get; set; } // ObjectId of the Customer providing feedback

        [BsonElement("productId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ProductId { get; set; } // ObjectId of the Product related to this feedback

        [BsonElement("rating")]
        public float Rating { get; set; }

        [BsonElement("comment")]
        public string Comment { get; set; }

        [BsonElement("createdDate")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }

}
