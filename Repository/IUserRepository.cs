
/*
 * File: IUserRepository.cs
 * Project: HealthyBites
 * Description: This file defines the IUserRepository interface, which outlines the contract for operations on user data in the MongoDB database.
 *              It includes methods to retrieve, add, update, and delete user records, along with user-specific operations such as fetching by email.
 * 
 * Authors: Cooray N.T.L. it21177996 
 * 
 * Interface: IUserRepository
 * 
 * Methods:
 * - GetUserByIdAsync(string userId): Asynchronously retrieves a user by their unique identifier (userId).
 * - GetUsersAsync(int pageNumber, int pageSize): Retrieves a paginated list of users from the MongoDB database.
 * - GetTotalUsersAsync(): Returns the total count of users in the database.
 * - AddUserAsync(User user): Adds a new user to the MongoDB collection.
 * - UpdateUserAsync(string userId, User updatedUser): Updates an existing user's information in the database.
 * - DeleteUserAsync(string userId): Deletes a user from the database based on their unique identifier (userId).
 * - GetUserByEmailAsync(string email): Retrieves a user by their email address from the MongoDB collection.
 * 
 */

namespace HealthyBites.Repository
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(string userId);
        Task<List<User>> GetUsersAsync(int pageNumber, int pageSize);
        Task<long> GetTotalUsersAsync();
        Task AddUserAsync(User user);
        Task<bool> UpdateUserAsync(string userId, User updatedUser);
        Task<bool> DeleteUserAsync(string userId);
        Task<User> GetUserByEmailAsync(string email);
    }
}
