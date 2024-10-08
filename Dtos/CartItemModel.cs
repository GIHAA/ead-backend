/*
 * File: CartItemModel.cs
 * Project: Healthy Bites
 * Description: This file defines the CartItemModel for the Healthy Bites system. It represents individual items in a user's cart, including 
 *              product ID, quantity, and price.
 * 
 * Authors: Kuruppu K.A.G.S.R it21165252
 * 
 * Classes:
 * - CartItemModel: Model used for representing an item in the shopping cart in the Healthy Bites system.
 */

public class CartItemModel
{
    public string ProductId { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
}
