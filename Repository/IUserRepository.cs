/*
 * File: IUserRepository.cs
 * Project: TechFixBackend.Repository
 * Description: Interface for the UserRepository, defining the contract for data access operations related to users. 
 *              It includes methods for retrieving, creating, updating, and deleting users, as well as methods for 
 *              pagination, getting user details by email, and counting the total number of users.
 */

namespace TechFixBackend.Repository
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
