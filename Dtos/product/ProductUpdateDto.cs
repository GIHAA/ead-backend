using TechFixBackend._Models;

namespace TechFixBackend.Dtos
{
    public class ProductUpdateDto
    {
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string CategoryId { get; set; }
        public string VendorId { get; set; }
        public double Price { get; set; }
        public int StockQuantity { get; set; }
        public ProductStatus ProductStatus { get; set; }
        public string ProductImageUrl { get; set; }
    }
}
