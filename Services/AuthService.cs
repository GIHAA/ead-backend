
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TechFixBackend.Repository;

public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly string _key;

    public AuthService(IUserRepository userRepository, string key)
    {
        _userRepository = userRepository;
        _key = key;
    }

    // Register a new user
    public async Task RegisterAsync(string email, string password, string role = "customer")
    {
        var existingUser = await _userRepository.GetUserByEmailAsync(email);
        if (existingUser != null)
        {
            throw new Exception("Username or Email already exists");
        }

        var user = new User
        {
            Email = email,
            PasswordHash = HashPassword(password),
            Role = role,
            AccountCreationDate = DateTime.UtcNow
        };

        await _userRepository.AddUserAsync(user);
    }

    // Login an existing user
    public async Task<string> LoginAsync(string email, string password)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null || !VerifyPassword(password, user.PasswordHash))
        {
            throw new Exception("Invalid username or password");
        }

        // Generate JWT token if login is successful
        return GenerateJwtToken(user.Id, user.Role);
    }

    // Get a paginated list of users
    public async Task<(List<User> users, long totalUsers)> GetUsersAsync(int pageNumber, int pageSize)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;

        var users = await _userRepository.GetUsersAsync(pageNumber, pageSize);
        var totalUsers = await _userRepository.GetTotalUsersAsync();

        return (users, totalUsers);
    }

    // Get a user by their ID
    public async Task<User> GetUserByIdAsync(string userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        return user;
    }

    // Update user fields
    public async Task<bool> UpdateUserAsync(string userId, UserUpdateModel updateModel)
    {
        var existingUser = await _userRepository.GetUserByIdAsync(userId);
        if (existingUser == null)
        {
            throw new Exception("User not found");
        }

        // Update only provided fields
        existingUser = UpdateUserFields(existingUser, updateModel);

        // Attempt to update the user in the repository
        var updated = await _userRepository.UpdateUserAsync(userId, existingUser);
        if (!updated)
        {
            throw new Exception("User update failed");
        }

        // Return the update status
        return updated;
    }


    // Delete a user by their ID
    public async Task DeleteUserAsync(string userId)
    {
        var deleted = await _userRepository.DeleteUserAsync(userId);
        if (!deleted)
        {
            throw new Exception("User deletion failed");
        }
    }

    // Hash the password
    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
    }

    // Verify the password
    private bool VerifyPassword(string password, string storedHash)
    {
        return HashPassword(password) == storedHash;
    }

    // Generate a JWT token
    private string GenerateJwtToken(string userId, string role)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, userId),
                new Claim(ClaimTypes.Role, role)
            }),
            Expires = DateTime.UtcNow.AddHours(3),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    // Helper to update user fields
    private User UpdateUserFields(User existingUser, UserUpdateModel updateModel)
    {
        // Only update fields if they are provided (not null)
        if (!string.IsNullOrEmpty(updateModel.Email))
            existingUser.Email = updateModel.Email;

        if (!string.IsNullOrEmpty(updateModel.Role))
            existingUser.Role = updateModel.Role;

        if (!string.IsNullOrEmpty(updateModel.Name))
            existingUser.Name = updateModel.Name;

        if (!string.IsNullOrEmpty(updateModel.Address))
            existingUser.Address = updateModel.Address;

        if (!string.IsNullOrEmpty(updateModel.PhoneNumber))
            existingUser.PhoneNumber = updateModel.PhoneNumber;

        if (!string.IsNullOrEmpty(updateModel.Status))
            existingUser.Status = updateModel.Status;

        if (updateModel.VendorRating.HasValue)
            existingUser.VendorRating = updateModel.VendorRating.Value;

        return existingUser;
    }
}
