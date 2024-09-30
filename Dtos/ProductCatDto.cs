using TechFixBackend._Models;

namespace TechFixBackend.Dtos
{
    public class ProductCatDto
    {
        public string Id { get; set; }
        public string Category { get; set; }
        public string CatDescription { get; set; }
        public string ImageUrl { get; set; }
        public CategoryStatus CatStatus { get; set; }
    }
}
