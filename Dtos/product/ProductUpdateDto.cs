/*
 * File: ProductUpdateDto.cs
 * Project: TechFixBackend.Dtos
 * Description: Data Transfer Object (DTO) for updating an existing product. It contains optional fields such as 
 *              ProductName, ProductDescription, CategoryId, VendorId, Price, StockQuantity, ProductStatus, and 
 *              ProductImageUrl. This DTO is used when updating product details.
 */



using TechFixBackend._Models;

namespace TechFixBackend.Dtos
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
