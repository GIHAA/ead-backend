/*
 * File: CartItem.cs
 * Project: Healthy Bites
 * Description: This file defines the CartItem model for the Healthy Bites system. It represents individual items in a user's shopping cart,
 *              including the product ID, quantity, and price. The CartItem class is used to manage items before they are turned into an order.
 *              The data is stored in MongoDB, with necessary BSON attributes for proper serialization.
 * 
 * Authors: Kuruppu K.A.G.S.R it21165252
 * 
 * Classes:
 * - CartItem: Represents an item in a shopping cart, including product details, quantity, and price.
 */

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class CartItem
{
    [BsonElement("ProductId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ProductId { get; set; }

    [BsonElement("Quantity")]
    public int Quantity { get; set; } = 1;

    [BsonElement("Price")]
    public double Price { get; set; }
}