/*
 * File: ProductUpdateDto.cs
 * Project: Healthy Bites
 * Description: This file defines the ProductUpdateDto data transfer object for the Healthy Bites system. It represents the data required to update 
 *              an existing product, including product name, description, category ID, vendor ID, price, stock quantity, product status, and image URL.
 * 
 * Authors: Kuruppu K.A.G.S.R it21165252
 * 
 * Classes:
 * - ProductUpdateDto: Data transfer object used for updating an existing product in the Healthy Bites system.
 */

using HealthyBites._Models;

namespace HealthyBites.Dtos
{
    public class ProductUpdateDto
    {
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public string? CategoryId { get; set; }
        public string? VendorId { get; set; }
        public double Price { get; set; }
        public int StockQuantity { get; set; }
        public ProductStatus ProductStatus { get; set; }
        public string? ProductImageUrl { get; set; }
    }
}
