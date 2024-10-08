/*
 * File: ProductWithVendorDto.cs
 * Project: Healthy Bites
 * Description: This file defines the ProductWithVendorDto data transfer object for the Healthy Bites system. It represents product data along with 
 *              vendor information, including product details such as name, description, category, price, stock, status, and image URL.
 * 
 * Authors: Kuruppu K.A.G.S.R it21165252
 * 
 * Classes:
 * - ProductWithVendorDto: Data transfer object that includes product details and associated vendor information for the Healthy Bites system.
 */


using TechFixBackend._Models;

namespace TechFixBackend.Dtos
{
    public class ProductWithVendorDto
    {
        public string Id { get; set; }
        public User Vendor { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public ProductCat Category { get; set; }
        public double Price { get; set; }
        public int StockQuantity { get; set; }
        public ProductStatus ProductStatus { get; set; }
        public string ProductImageUrl { get; set; }


    }
}
