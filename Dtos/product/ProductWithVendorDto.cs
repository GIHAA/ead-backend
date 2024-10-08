/*
 * File: ProductWithVendorDto.cs
 * Project: Healthy Bites.Dtos
 * Description: Data Transfer Object (DTO) that represents a product along with its vendor information. 
 *              It includes properties such as Id, Vendor, ProductName, ProductDescription, Category, Price, 
 *              StockQuantity, ProductStatus, and ProductImageUrl. This DTO is used when returning product data 
 *              that includes vendor details.
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
