/*
 * File: FeedbackCreateDto.cs
 * Project: Healthy Bites.Dtos
 * Description: Data Transfer Object (DTO) for creating new feedback. This class contains properties 
 *              such as VendorId, ProductId, Rating, and Comment that are required for creating feedback.
 */


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
