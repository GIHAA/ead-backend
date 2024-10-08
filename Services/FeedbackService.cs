/*
 * File: FeedbackService.cs
 * Project: Healthy Bites.Services
 * Description: Service responsible for handling business logic related to feedback, including adding, updating,
 *              and retrieving feedback. It also manages the calculation of vendor ratings and ensures feedback
 *              validity through interactions with repositories for users, products, and feedback.
 */



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
        public async Task AddFeedbackAsync(FeedbackCreateDto feedbackCreateDto, String id)
        {
            // Validate if the customer exists
            var customer = await _userRepository.GetUserByIdAsync(id);
            if (customer == null)
                throw new Exception("Customer not found.");

            // Validate the Vendor and ensure the role is vendor
            var vendor = await _userRepository.GetUserByIdAsync(feedbackCreateDto.VendorId);
            if (vendor == null || vendor.Role != "vendor")
            {
                throw new Exception("Invalid vendor ID or the user is not a vendor.");
            }

            // Validate the Product and ensure it belongs to the Vendor
            var product = await _productRepository.GetProductByIdAsync(feedbackCreateDto.ProductId);
            if (product == null || product.VendorId != feedbackCreateDto.VendorId)
            {
                throw new Exception("Invalid product ID or the product does not belong to the specified vendor.");
            }

            // Check if the customer has already provided feedback for this product
            bool hasProvidedFeedback = await HasCustomerProvidedFeedbackAsync(id, feedbackCreateDto.ProductId);
            if (hasProvidedFeedback)
            {
                throw new Exception("Customer has already provided feedback for this product.");
            }


            // Create the feedback object with validated data
            var feedback = new Feedback
            {
                VendorId = vendor.Id,
                CustomerId = id,
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

        public async Task<(List<FeedbackWithDetailsDto> feedbacks, long totalFeedbacks)> GetAllFeedbackAsync(int pageNumber, int pageSize, string search = "")
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            // Retrieve paginated feedbacks from repository with optional search
            var feedbacks = await _feedbackRepository.GetFeedbacksAsync(pageNumber, pageSize, search);
            var feedbacksWithDetails = new List<FeedbackWithDetailsDto>();

            foreach (var feedback in feedbacks)
            {
                // Fetch customer and vendor details
                var customer = await _userRepository.GetUserByIdAsync(feedback.CustomerId);
                var vendor = await _userRepository.GetUserByIdAsync(feedback.VendorId);
                var product = await _productRepository.GetProductByIdAsync(feedback.ProductId);

                // Map feedback to include customer and vendor details
                var feedbackWithDetails = new FeedbackWithDetailsDto
                {
                    Id = feedback.Id,
                    Customer = customer, // Populate customer details
                    Vendor = vendor,     // Populate vendor details
                    Product = product,   // Populate product details
                    Rating = feedback.Rating,
                    Comment = feedback.Comment,
                    CreatedDate = feedback.CreatedDate
                };

                feedbacksWithDetails.Add(feedbackWithDetails);
            }

            // Get total count of feedbacks for pagination purposes
            var totalFeedbacks = await _feedbackRepository.GetTotalFeedbacksAsync(search);

            return (feedbacksWithDetails, totalFeedbacks);
        }

        // Retrieve a single feedback for a specific customer, product, and vendor
        public async Task<FeedbackWithDetailsDto> GetFeedbackForCustomerProductVendorAsync(string vendorId, string productId, string customerId)
        {
            // Fetch the feedback from the repository
            var feedback = await _feedbackRepository.GetFeedbackForCustomerProductVendorAsync(vendorId, productId, customerId);

            if (feedback == null)
            {
                return null;
            }

            // Fetch customer and vendor details
            var customer = await _userRepository.GetUserByIdAsync(customerId);
            var vendor = await _userRepository.GetUserByIdAsync(vendorId);
            var product = await _productRepository.GetProductByIdAsync(productId);

            // Map the feedback entity to a DTO and return it
            return new FeedbackWithDetailsDto
            {
                Id = feedback.Id,
                Customer = customer,
                Vendor = vendor,
                Product = product,
                Rating = feedback.Rating,
                Comment = feedback.Comment,
                CreatedDate = feedback.CreatedDate
            };
        }


        // Get all feedback for a vendor
        public async Task<List<FeedbackWithDetailsDto>> GetFeedbackForVendorAsync(string vendorId)
        {

            var feedbacks = await _feedbackRepository.GetFeedbackByVendorIdAsync(vendorId);
            var feedbacksWithDetails = new List<FeedbackWithDetailsDto>();

            if (feedbacks == null)
            {
                return null;
            }



            foreach (var feedback in feedbacks)
            {
                // Fetch customer and vendor details
                var customer = await _userRepository.GetUserByIdAsync(feedback.CustomerId);
                var vendor = await _userRepository.GetUserByIdAsync(feedback.VendorId);
                var product = await _productRepository.GetProductByIdAsync(feedback.ProductId);

                // Map feedback to include customer and vendor details
                var feedbackWithDetails = new FeedbackWithDetailsDto
                {
                    Id = feedback.Id,
                    Customer = customer, // Populate customer details
                    Vendor = vendor,     // Populate vendor details
                    Product = product,   // Populate product details
                    Rating = feedback.Rating,
                    Comment = feedback.Comment,
                    CreatedDate = feedback.CreatedDate
                };

                feedbacksWithDetails.Add(feedbackWithDetails);
            }

            return feedbacksWithDetails;
        }

        // Get all feedback for a product
        public async Task<List<FeedbackDto>> GetFeedbackForProductAsync(string productId)
        {
            var feedbacks = await _feedbackRepository.GetFeedbackByProductIdAsync(productId);
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