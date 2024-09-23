using MongoDB.Driver;
using TechFixBackend._Models;

namespace TechFixBackend.Repository
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly IMongoCollection<Feedback> _feedbackCollection;
        private readonly IMongoCollection<User> _userCollection;

        public FeedbackRepository(MongoDBContext context)
        {
            _feedbackCollection = context.Feedback;  
            _userCollection = context.Users;         
        }

        // Add feedback to the Feedback collection
        public async Task AddFeedbackAsync(Feedback feedback)
        {
            await _feedbackCollection.InsertOneAsync(feedback);
        }

        // Get all feedback by vendor ID
        public async Task<List<Feedback>> GetFeedbackByVendorIdAsync(string vendorId)
        {
            return await _feedbackCollection.Find(fb => fb.VendorId == vendorId).ToListAsync();
        }

        // Get feedback by feedback ID
        public async Task<Feedback> GetFeedbackByIdAsync(string feedbackId)
        {
            return await _feedbackCollection.Find(fb => fb.Id == feedbackId).FirstOrDefaultAsync();
        }

        // Update feedback by feedback ID and customer ID
        public async Task UpdateFeedbackAsync(string feedbackId, string customerId, string newComment, float newRating)
        {
            var filter = Builders<Feedback>.Filter.Where(fb => fb.Id == feedbackId && fb.CustomerId == customerId);
            var update = Builders<Feedback>.Update
                .Set(fb => fb.Comment, newComment)
                .Set(fb => fb.Rating, newRating);

            await _feedbackCollection.UpdateOneAsync(filter, update);
        }

        // Check if a customer has already provided feedback for a specific product
        public async Task<bool> HasCustomerProvidedFeedbackAsync(string customerId, string productId)
        {
            var filter = Builders<Feedback>.Filter.Where(fb => fb.CustomerId == customerId && fb.ProductId == productId);
            var count = await _feedbackCollection.CountDocumentsAsync(filter);
            return count > 0;
        }

        // Update the average rating of a vendor in the User collection
        public async Task UpdateUserAverageRating(string vendorId, float avgRating)
        {
            var filter = Builders<User>.Filter.Where(u => u.Id == vendorId && u.Role == "vendor");
            var update = Builders<User>.Update.Set(u => u.AverageRating, avgRating);

            await _userCollection.UpdateOneAsync(filter, update);
        }
    }
}
