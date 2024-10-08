/*
 * File: ProductCreateDto.cs
 * Project: TechFixBackend.Dtos
 * Description: Data Transfer Object (DTO) for creating a new product. It contains properties such as VendorId, 
 *              ProductName, ProductDescription, CategoryId, Price, StockQuantity, and ProductImageUrl. 
 *              This DTO is used when submitting product creation requests.
 */


namespace TechFixBackend.Dtos
{
    public class ProductCreateDto
    {
        public string VendorId  { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string CategoryId { get; set; }
        public double Price { get; set; }
        public int StockQuantity { get; set; }
        public string ProductImageUrl { get; set; }
    }
}