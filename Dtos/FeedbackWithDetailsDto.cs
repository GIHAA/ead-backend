using TechFixBackend._Models;

namespace TechFixBackend.Dtos
{
    public class FeedbackWithDetailsDto
    {
        public string Id { get; set; }
        public User Customer { get; set; }   // Includes customer details
        public User Vendor { get; set; }     // Includes vendor details
        public Product Product { get; set; } // Includes product details
        public float Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedDate { get; set; }
    }

}
