
/*
 * File: UserRepository.cs
 * Project: HealthyBites
 * Description: This file defines the UserRepository class, which handles the database operations for user data in MongoDB.
 *              It includes methods to retrieve, add, update, and delete user records in the MongoDB collection.
 * 
 * Authors: Cooray N.T.L. it21177996 
 * 
 * Classes:
 * - UserRepository: Provides an interface between the MongoDB database and the application, managing user-related operations.
 * 
 * Methods:
 * - GetUserByIdAsync: Retrieves a user by their unique userId.
 * - GetUsersAsync: Retrieves a paginated list of users.
 * - GetTotalUsersAsync: Returns the total number of users in the database.
 * - AddUserAsync: Adds a new user to the database.
 * - UpdateUserAsync: Updates an existing user's details.
 * - DeleteUserAsync: Deletes a user from the database.
 * - GetUserByEmailAsync: Retrieves a user by their email address.
 * 
 */

using MongoDB.Driver;
namespace HealthyBites.Repository
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
