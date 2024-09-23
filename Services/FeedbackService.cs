using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechFixBackend.Dtos;
using TechFixBackend._Models;
using TechFixBackend.Repository;

namespace TechFixBackend.Services
{
    public class FeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;

        public FeedbackService(IFeedbackRepository feedbackRepository, IUserRepository userRepository, IProductRepository productRepository)
        {
            _feedbackRepository = feedbackRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
        }

        // Add feedback with proper validation of VendorId, CustomerId, and ProductId
        public async Task AddFeedbackAsync(FeedbackCreateDto feedbackCreateDto)
        {
            // The IDs for Vendor, Customer, and Product are provided by the client-side and should be validated here.

            // Validate the Vendor and ensure the role is vendor
            var vendor = await _userRepository.GetUserByIdAsync(feedbackCreateDto.VendorId);
            if (vendor == null || vendor.Role != "vendor")
            {
                throw new Exception("Invalid vendor ID or the user is not a vendor.");
            }

            // Validate the Customer and ensure the role is customer
            var customer = await _userRepository.GetUserByIdAsync(feedbackCreateDto.CustomerId);
            if (customer == null || customer.Role != "customer")
            {
                throw new Exception("Invalid customer ID.");
            }

            // Validate the Product and ensure it belongs to the Vendor
            var product = await _productRepository.GetProductByIdAsync(feedbackCreateDto.ProductId);
            if (product == null || product.VendorId != feedbackCreateDto.VendorId)
            {
                throw new Exception("Invalid product ID or the product does not belong to the specified vendor.");
            }

            // Create the feedback object with validated data
            var feedback = new Feedback
            {
                VendorId = vendor.Id,
                CustomerId = customer.Id,
                ProductId = product.Id,
                Rating = feedbackCreateDto.Rating,
                Comment = feedbackCreateDto.Comment,
                CreatedDate = DateTime.UtcNow
            };

            // Add the feedback and update the average rating for the vendor
            await _feedbackRepository.AddFeedbackAsync(feedback);
            await UpdateVendorAverageRating(vendor.Id);
        }

        // Update feedback to modify the comment and rating
        public async Task UpdateFeedbackAsync(string feedbackId, string customerId, FeedbackUpdateDto feedbackUpdateDto)
        {
            // Retrieve feedback
            var feedback = await _feedbackRepository.GetFeedbackByIdAsync(feedbackId);

            // Validate feedback ownership by customer
            if (feedback == null || feedback.CustomerId != customerId)
            {
                throw new Exception("Feedback not found or not owned by the customer.");
            }

            // Update comment and rating
            feedback.Comment = feedbackUpdateDto.Comment;
            feedback.Rating = feedbackUpdateDto.Rating;

            // Update feedback and recalculate the average rating for the vendor
            await _feedbackRepository.UpdateFeedbackAsync(feedbackId, customerId, feedback.Comment, feedback.Rating);
            await UpdateVendorAverageRating(feedback.VendorId);
        }

        // Recalculate and update the vendor's average rating
        private async Task UpdateVendorAverageRating(string vendorId)
        {
            // Fetch all feedback for the vendor
            var feedbackList = await _feedbackRepository.GetFeedbackByVendorIdAsync(vendorId);

            // Calculate average rating
            float avgRating = feedbackList.Any() ? feedbackList.Average(fb => fb.Rating) : 0;

            // Update the vendor's average rating in the User collection
            await _feedbackRepository.UpdateUserAverageRating(vendorId, avgRating);
        }

        // Get all feedback for a vendor
        public async Task<List<FeedbackDto>> GetFeedbackForVendorAsync(string vendorId)
        {
            var feedbacks = await _feedbackRepository.GetFeedbackByVendorIdAsync(vendorId);
            return feedbacks.ConvertAll(fb => new FeedbackDto
            {
                VendorId = fb.VendorId,
                CustomerId = fb.CustomerId,
                ProductId = fb.ProductId,
                Rating = fb.Rating,
                Comment = fb.Comment,
                CreatedDate = fb.CreatedDate
            });
        }

        // Get the average rating of a vendor
        public async Task<float?> GetVendorAverageRatingAsync(string vendorId)
        {
            var feedbackList = await _feedbackRepository.GetFeedbackByVendorIdAsync(vendorId);
            return feedbackList.Any() ? feedbackList.Average(fb => fb.Rating) : (float?)null;
        }

        // Check if a customer has already provided feedback for a product
        public async Task<bool> HasCustomerProvidedFeedbackAsync(string customerId, string productId)
        {
            return await _feedbackRepository.HasCustomerProvidedFeedbackAsync(customerId, productId);
        }
    }
}
