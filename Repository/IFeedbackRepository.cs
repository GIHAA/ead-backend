using System.Collections.Generic;
using System.Threading.Tasks;
using TechFixBackend._Models;

namespace TechFixBackend.Repository
{
    public interface IFeedbackRepository
    {
        Task<List<Feedback>> GetFeedbacksAsync(int pageNumber, int pageSize);
        Task<Feedback> GetFeedbackByIdAsync(string productId);
        Task CreateFeedbackAsync(Feedback product);
        Task<bool> UpdateFeedbackAsync(string productId, Feedback updatedFeedback);
        Task<bool> DeleteFeedbackAsync(string productId);
    }
}
