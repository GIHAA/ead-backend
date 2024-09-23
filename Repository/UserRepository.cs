using MongoDB.Driver;
namespace TechFixBackend.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _users;

        public UserRepository(MongoDBContext context)
        {
            _users = context.Users;
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await _users.Find(u => u.Id == userId).FirstOrDefaultAsync();
        }

        public async Task<List<User>> GetUsersAsync(int pageNumber, int pageSize)
        {
            return await _users
                .Find(u => true)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();
        }

        public async Task<long> GetTotalUsersAsync()
        {
            return await _users.CountDocumentsAsync(u => true);
        }

        public async Task AddUserAsync(User user)
        {
            if (user.Role != "vendor")
            {
                user.AverageRating = null;
            }
            await _users.InsertOneAsync(user);
        }

        public async Task<bool> UpdateUserAsync(string userId, User updatedUser)
        {
            if (updatedUser.Role != "vendor")
            {
                updatedUser.AverageRating = null;
            }
            var result = await _users.ReplaceOneAsync(u => u.Id == userId, updatedUser);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var result = await _users.DeleteOneAsync(u => u.Id == userId);
            return result.DeletedCount > 0;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _users.Find(u => u.Email == email).FirstOrDefaultAsync();
        }
    }
}
