/*
 * File: IFeedbackRepository.cs
 * Project: Healthy Bites.Repository
 * Description: Interface for the FeedbackRepository, defining the contract for feedback-related data access operations.
 *              It provides methods for adding, updating, retrieving feedback, checking if feedback exists, 
 *              and updating user (vendor) ratings.
 */



using HealthyBites._Models;

namespace HealthyBites.Repository
{
    public interface IFeedbackRepository
    {
        Task AddFeedbackAsync(Feedback feedback);
        Task<List<Feedback>> GetFeedbackByVendorIdAsync(string vendorId);
        Task<List<Feedback>> GetFeedbackByProductIdAsync(string productId);
        Task<List<Feedback>> GetFeedbacksAsync(int pageNumber, int pageSize, string search = "");
        Task<Feedback> GetFeedbackForCustomerProductVendorAsync(string vendorId, string productId, string customerId);
        Task<long> GetTotalFeedbacksAsync(string search = "");
        Task<Feedback> GetFeedbackByIdAsync(string feedbackId);
        Task UpdateFeedbackAsync(string feedbackId, string customerId, string newComment, float newRating);
        Task<bool> HasCustomerProvidedFeedbackAsync(string customerId, string productId);
        Task UpdateUserAverageRating(string vendorId, float avgRating);
    }
}
