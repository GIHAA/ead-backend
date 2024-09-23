namespace TechFixBackend.Dtos
{
    public class FeedbackDto
    {
        public string Id { get; set; } 
        public string VendorId { get; set; } 
        public string CustomerId { get; set; } 
        public string ProductId { get; set; } 
        public float Rating { get; set; } 
        public string Comment { get; set; } 
        public DateTime CreatedDate { get; set; } 
    }
}
