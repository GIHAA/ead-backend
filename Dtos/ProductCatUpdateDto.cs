using TechFixBackend._Models;

namespace TechFixBackend.Dtos
{
    public class ProductCatUpdateDto
    {
        public string? catName { get; set; }
        public string? catDescription { get; set; }
        public string? catImageUrl { get; set; }
        public CategoryStatus? catStatus { get; set; }
    }
}
