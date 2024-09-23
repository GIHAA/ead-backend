
using TechFixBackend._Models;

namespace TechFixBackend.Repository
{
    public interface IFeedbackRepository
    {
        Task AddFeedbackAsync(Feedback feedback);
        Task<List<Feedback>> GetFeedbackByVendorIdAsync(string vendorId);
        Task<Feedback> GetFeedbackByIdAsync(string feedbackId);
        Task UpdateFeedbackAsync(string feedbackId, string customerId, string newComment, float newRating);
        Task<bool> HasCustomerProvidedFeedbackAsync(string customerId, string productId);
        Task UpdateUserAverageRating(string vendorId, float avgRating);
    }
}
