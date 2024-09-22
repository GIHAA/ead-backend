namespace TechFixBackend.Dtos
{
    public class ProductUpdateDto
    {
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }
        public int StockQuantity { get; set; }
        public string ProductStatus { get; set; }
        public string ProductImageUrl { get; set; }
    }
}
