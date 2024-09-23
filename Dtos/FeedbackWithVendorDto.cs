using TechFixBackend._Models;

namespace TechFixBackend.Dtos
{
    public class FeedbackWithVendorDto
    {
        public string Id { get; set; }
        public User Vendor { get; set; }
        public User Customer { get; set; }
        public Product Product { get; set; }
        public string FeedbackMessage { get; set; }
        public int Rating { get; set; }
    }
}
