using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string Email { get; set; }
    
    public string PasswordHash { get; set; } 
    
    public string Role { get; set; } = "customer"; // "customer" ,  "admin" , "csr" and "admin"

    public string Name { get; set; }
    
    public string Address { get; set; }
    
    public string PhoneNumber { get; set; }
    
    public string Status { get; set; }
    
    public DateTime AccountCreationDate { get; set; } = DateTime.UtcNow;
    
}
