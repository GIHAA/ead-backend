namespace TechFixBackend.Dtos
{
    public class FeedbackCreateDto
    {
        public string VendorId { get; set; }
        public string ProductId { get; set; }
        public float Rating { get; set; } 
        public string Comment { get; set; } 
    }
}
