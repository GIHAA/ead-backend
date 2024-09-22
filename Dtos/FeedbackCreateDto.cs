namespace TechFixBackend.Dtos
{
    public class FeedbackCreateDto
    {
        public string VendorId { get; set; }
        public string ProductId { get; set; }
        public string FeedbackMessage { get; set; }
        public int Rating { get; set; }
    }
}
