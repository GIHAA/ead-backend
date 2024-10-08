/*
 * File: FeedbackController.cs
 * Project: Healthy Bites.Controllers
 * Description: Controller for managing feedback-related operations such as retrieving feedbacks, adding new feedback, 
 *              updating feedback, and fetching feedback for a vendor or product. It also handles JWT token validation 
 *              to ensure proper authentication for each request.
 *              The controller interacts with the FeedbackService for executing business logic.
 */


using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HealthyBites.Dtos;
using HealthyBites.Services;
using System.IdentityModel.Tokens.Jwt;

namespace HealthyBites.Controllers
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

        [HttpGet]
        public async Task<IActionResult> GetFeedbacks(int pageNumber = 1, int pageSize = 10, string search = "")
        {
            try
            {
                // Validate page number and page size
                if (pageNumber < 1)
                {
                    return BadRequest(new { Message = "Page number must be greater than 0." });
                }

                if (pageSize < 1)
                {
                    return BadRequest(new { Message = "Page size must be greater than 0." });
                }

                // Get feedbacks and total count with search
                var (pagedFeedbacks, totalFeedbacks) = await _feedbackService.GetAllFeedbackAsync(pageNumber, pageSize, search);

                // Check if no feedbacks are found
                if (pagedFeedbacks == null || !pagedFeedbacks.Any())
                {
                    return NotFound(new { Message = "No feedbacks found." });
                }

                // Calculate total pages
                int totalPages = (int)Math.Ceiling((double)totalFeedbacks / pageSize);

                // Ensure pageNumber does not exceed totalPages
                if (pageNumber > totalPages && totalPages > 0)
                {
                    return BadRequest(new { Message = "Page number exceeds total pages." });
                }

                var response = new
                {
                    TotalRecords = totalFeedbacks,
                    TotalPages = totalPages,
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    Feedbacks = pagedFeedbacks
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving feedbacks.", Details = ex.Message });
            }
        }

        // Get a single feedback for a specific product and vendor by the customer identified by the token
        [HttpGet("vendor/{vendorId}/product/{productId}")]
        public async Task<IActionResult> GetFeedbackForCustomerProductVendor(string vendorId, string productId)
        {
            try
            {
                // Extract token from the Authorization header
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { Message = "Token is missing." });
                }

                // Decode the JWT token
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                // Extract customerId (e.g., claim type 'nameid')
                var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "nameid");
                if (userIdClaim == null)
                {
                    return Unauthorized(new { Message = "User ID not found in the token." });
                }

                var customerId = userIdClaim.Value;

                // Use the service method to get the feedback for the specified product, vendor, and customer
                var feedback = await _feedbackService.GetFeedbackForCustomerProductVendorAsync(vendorId, productId, customerId);

                if (feedback == null)
                {
                    return NotFound(new { Message = "No feedback found for this product, vendor, and customer." });
                }

                return Ok(feedback);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving feedback.", Details = ex.Message });
            }
        }


        // Add new feedback
        [HttpPost]
        public async Task<IActionResult> AddFeedback([FromBody] FeedbackCreateDto feedbackCreateDto)
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { Message = "Token is missing." });
                }

                // Decode the token
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                // Console.WriteLine(jwtToken);

                // Extract the user ID from the token
                var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "nameid");
                if (userIdClaim == null)
                {
                    return Unauthorized(new { Message = "User ID not found in the token." });
                }

                var userId = userIdClaim.Value;
                Console.WriteLine(userId);

                // Pass the user ID to the service method
                await _feedbackService.AddFeedbackAsync(feedbackCreateDto, userId);
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
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { Message = "Token is missing." });
                }

                // Decode the token
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                // Extract the user ID from the token
                var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "nameid");
                if (userIdClaim == null)
                {
                    return Unauthorized(new { Message = "User ID not found in the token." });
                }

                var customerId = userIdClaim.Value;

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
        [HttpGet("vendor-feedback")]
        public async Task<IActionResult> GetFeedbackForVendor()
        {

            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                // Console.WriteLine(token);

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { Message = "Token is missing." });
                }

                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var vendorIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "nameid");
                if (vendorIdClaim == null)
                {
                    return Unauthorized(new { Message = "Vendor ID not found in the token." });
                }

                var vendorId = vendorIdClaim.Value;

                var feedbacks = await _feedbackService.GetFeedbackForVendorAsync(vendorId);

                if (feedbacks == null || !feedbacks.Any())
                {
                    return NotFound(new { Message = "No feedbacks found for the vendor." });
                }

                return Ok(feedbacks);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        // Get all feedback for a specific product
        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetFeedbackForProduct(string productId)
        {
            try
            {
                var feedbacks = await _feedbackService.GetFeedbackForProductAsync(productId);
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
