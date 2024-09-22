using System.Collections.Generic;
using System.Threading.Tasks;
using TechFixBackend._Models;
using TechFixBackend.Dtos;

namespace TechFixBackend.Services
{
    public interface IFeedbackService
    {
        Task<List<FeedbackWithVendorDto>> GetAllFeedbacksAsync(int pageNumber, int pageSize);
        Task<FeedbackWithVendorDto> GetFeedbackByIdAsync(string productId);
        Task<Feedback> CreateFeedbackAsync(FeedbackCreateDto productDto);
        Task<bool> UpdateFeedbackAsync(string productId, FeedbackUpdateDto productDto);
        Task<bool> DeleteFeedbackAsync(string productId);
    }
}
