using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechFixBackend.Dtos;
using TechFixBackend.Services;

namespace TechFixBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly FeedbackService _feedbackService;

        public FeedbackController(FeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        // Add new feedback
        [HttpPost]
        public async Task<IActionResult> AddFeedback([FromBody] FeedbackCreateDto feedbackCreateDto)
        {
            try
            {
                // Add feedback using the service
                await _feedbackService.AddFeedbackAsync(feedbackCreateDto);
                return CreatedAtAction(nameof(GetFeedbackForVendor), new { vendorId = feedbackCreateDto.VendorId }, feedbackCreateDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        // Update feedback comment and rating
        [HttpPut("{feedbackId}")]
        public async Task<IActionResult> UpdateFeedback(string feedbackId, [FromBody] FeedbackUpdateDto feedbackUpdateDto)
        {
            try
            {
                // Assume customerId is extracted from the current user context (replace with actual retrieval method)
                string customerId = "60f0fcb9874bb37187ba7383"; // Replace with actual customer ID retrieval

                // Update feedback using the service
                await _feedbackService.UpdateFeedbackAsync(feedbackId, customerId, feedbackUpdateDto);
                return Ok(new { Message = "Feedback updated successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // Get all feedback for a specific vendor
        [HttpGet("vendor/{vendorId}")]
        public async Task<IActionResult> GetFeedbackForVendor(string vendorId)
        {
            try
            {
                var feedbacks = await _feedbackService.GetFeedbackForVendorAsync(vendorId);
                return Ok(feedbacks);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // Get the average rating of a vendor
        [HttpGet("vendor/{vendorId}/average-rating")]
        public async Task<IActionResult> GetVendorAverageRating(string vendorId)
        {
            try
            {
                var averageRating = await _feedbackService.GetVendorAverageRatingAsync(vendorId);
                if (averageRating == null)
                {
                    return NotFound(new { Message = "Vendor not found." });
                }

                return Ok(new { AverageRating = averageRating });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // Check if a customer has provided feedback for a specific product
        [HttpGet("check")]
        public async Task<IActionResult> CheckIfCustomerProvidedFeedback([FromQuery] string customerId, [FromQuery] string productId)
        {
            try
            {
                bool hasProvidedFeedback = await _feedbackService.HasCustomerProvidedFeedbackAsync(customerId, productId);
                return Ok(new { HasProvidedFeedback = hasProvidedFeedback });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
