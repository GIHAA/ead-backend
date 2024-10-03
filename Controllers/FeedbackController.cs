﻿using Microsoft.AspNetCore.Mvc;
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
