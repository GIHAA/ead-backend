/*
 * File: CartItemWithProduct.cs
 * Project: Healthy Bites
 * Description: This file defines the CartItemWithProduct model for the Healthy Bites system. It represents a shopping cart item with detailed 
 *              product information, including product ID, name, description, price, quantity, and image URL.
 * 
 * Authors: Kuruppu K.A.G.S.R it21165252
 * 
 * Classes:
 * - CartItemWithProduct: Model used for representing a cart item with associated product details in the Healthy Bites system.
 */

public class CartItemWithProduct
{
    public string ProductId { get; set; }
    public string ProductName { get; set; }
    public string ProductDescription { get; set; }
    public double Price { get; set; }
    public int Quantity { get; set; }
    public string ProductImageUrl { get; set; }
}