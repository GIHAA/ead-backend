/*
 * File: FeedbackWithDetailsDto.cs
 * Project: Healthy Bites.Dtos
 * Description: Data Transfer Object (DTO) for feedback that includes detailed information about the feedback, 
 *              such as customer, vendor, and product details. It provides additional context beyond the basic 
 *              feedback data with properties like Id, Rating, Comment, and CreatedDate.
 */


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
