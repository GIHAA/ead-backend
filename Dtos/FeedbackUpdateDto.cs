/*
 * File: FeedbackUpdateDto.cs
 * Project: Healthy Bites.Dtos
 * Description: Data Transfer Object (DTO) for updating existing feedback. This class contains the properties 
 *              Rating and Comment, which can be modified by the customer when updating their feedback.
 */

namespace TechFixBackend.Dtos
{
    public class FeedbackUpdateDto
    {
        public float Rating { get; set; }
        public string Comment { get; set; }
    }
}
