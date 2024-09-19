using MongoDB.Driver;
using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

public class AuthService
{
    private readonly IMongoCollection<User> _users;
    private readonly string _key;
    private readonly object _vendorRepository;
    private readonly object _authService;

    public AuthService(MongoDBContext context, string key)
    {
        _users = context.Users;
        _key = key;
    }

    // Register a new user
    public void Register(string username, string email, string password, string role = "customer")
    {
        var existingUser = _users.Find(u => u.Email == email).FirstOrDefault();
        if (existingUser != null)
        {
            throw new Exception("Username or Email already exists");
        }

        var user = new User
        {
            Email = email,
            PasswordHash = HashPassword(password),
            Role = role
        };

        _users.InsertOne(user);
    }

    // Login an existing user
    public string Login(string email, string password)
    {
        var user = _users.Find(u => u.Email == email).FirstOrDefault();
        if (user == null || !VerifyPassword(password, user.PasswordHash))
        {
            throw new Exception("Invalid username or password");
        }

        // Generate JWT token if login is successful
        return GenerateJwtToken(user.Id, user.Role);
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

    public (List<User> users, long totalUsers) GetUsers(int pageNumber, int pageSize)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;

        // Get total count of users
        long totalUsers = _users.CountDocuments(u => true);

        // Fetch paginated users
        var pagedUsers = _users
            .Find(u => true)
            .Skip((pageNumber - 1) * pageSize)
            .Limit(pageSize)
            .ToList();

        return (pagedUsers, totalUsers);
    }


    public User GetUserById(string userId)
    {
        return _users.Find(u => u.Id == userId).FirstOrDefault();
    }

    public User UpdateUserFields(User existingUser, UserUpdateModel updateModel)
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

    public void UpdateUser(string userId, User updatedUser)
    {
        _users.ReplaceOne(u => u.Id == userId, updatedUser);
    }

    public void DeleteUser(string userId)
    {
        _users.DeleteOne(u => u.Id == userId);
    }

    public async Task<User> GetUserByIdAsync(string userId)
    {
        return await _users.Find(u => u.Id == userId).FirstOrDefaultAsync();
    }

    public async Task<bool> UpdateUserAsync(string userId, User updatedUser)
    {
        var result = await _users.ReplaceOneAsync(u => u.Id == userId, updatedUser);
        return result.ModifiedCount > 0;
    }

   

}
