/*
 * File: ProductCreateDto.cs
 * Project: Healthy Bites
 * Description: This file defines the ProductCreateDto data transfer object for the Healthy Bites system. It represents the data required to create 
 *              a new product, including vendor ID, product name, description, category ID, price, stock quantity, and image URL.
 * 
 * Authors: Kuruppu K.A.G.S.R it21165252
 * 
 * Classes:
 * - ProductCreateDto: Data transfer object used for creating a new product in the Healthy Bites system.
 */

namespace HealthyBites.Dtos
{
    public class ProductCreateDto
    {
        public string VendorId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string CategoryId { get; set; }
        public double Price { get; set; }
        public int StockQuantity { get; set; }
        public string ProductImageUrl { get; set; }
    }
}