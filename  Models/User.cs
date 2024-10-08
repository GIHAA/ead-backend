/*
 * File: User.cs
 * Project: Healthy Bites._Models
 * Description: Represents the User model in the database. This class includes properties such as Id, Email, PasswordHash,
 *              Role (customer, admin, vendor, csr), Name, Address, PhoneNumber, Cart, and Status. It also tracks the 
 *              account creation date, and optionally includes AverageRating for vendor roles and TotalPrice for customer roles.
 */

/*
 * File: User.cs
 * Project: HealthyBites
 * Description: This file defines the User class which represents the structure of user data stored in the MongoDB database.
 *              It includes properties such as email, password hash, role, cart items, and user status.
 * 
 * Authors: Cooray N.T.L. it21177996
 * 
 * Classes:
 * - User: Represents the user model in the MongoDB database, storing essential details such as email, password hash, and role.
 * 
 * Properties:
 * - Id: The unique identifier for a user (MongoDB ObjectId).
 * - Email: The user's email address.
 * - PasswordHash: The hashed version of the user's password for security.
 * - Role: The role assigned to the user (customer, admin, vendor, csr).
 * - Cart: The list of cart items associated with the user.
 * - TotalPrice: Represents the total price of items in the user's cart.
 * - AverageRating: Represents the average rating for vendors.
 * 
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
