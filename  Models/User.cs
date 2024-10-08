/*
 * File: User.cs
 * Project: TechFixBackend._Models
 * Description: Represents the User model in the database. This class includes properties such as Id, Email, PasswordHash,
 *              Role (customer, admin, vendor, csr), Name, Address, PhoneNumber, Cart, and Status. It also tracks the 
 *              account creation date, and optionally includes AverageRating for vendor roles and TotalPrice for customer roles.
 */

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string Email { get; set; }

    public string PasswordHash { get; set; }

    public string Role { get; set; } = "customer"; // Roles: "customer", "admin", "vendor", "csr"

    public string Name { get; set; }

    public string Address { get; set; }

    public string PhoneNumber { get; set; }

    [BsonElement("Cart")]
    public List<CartItem> Cart { get; set; } = new List<CartItem>();

    public string Status { get; set; }

    public DateTime AccountCreationDate { get; set; } = DateTime.UtcNow;

    // Ensure AverageRating is set and used only for vendor roles
    [BsonIgnoreIfNull]
    public float? AverageRating { get; set; }

    // Ensure TotalPrice is set and used only for customer roles
    [BsonElement("TotalPrice")]
    public double TotalPrice { get; set; } = 0;
}
