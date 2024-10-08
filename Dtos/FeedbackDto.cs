/*
 * File: FeedbackDto.cs
 * Project: Healthy Bites.Dtos
 * Description: Data Transfer Object (DTO) representing feedback details. This class includes properties like
 *              Id, VendorId, CustomerId, ProductId, Rating, Comment, and CreatedDate, providing a complete 
 *              structure for feedback data when fetching or displaying feedback information.
 */

namespace HealthyBites.Dtos
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
