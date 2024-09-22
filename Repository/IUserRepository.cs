

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
