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

    public AuthService(MongoDBContext context, string key)
    {
        _users = context.Users;
        _key = key;
    }

    // Register a new user
    public void Register(string username, string email, string password)
    {
        var existingUser = _users.Find(u => u.Username == username || u.Email == email).FirstOrDefault();
        if (existingUser != null)
        {
            throw new Exception("Username or Email already exists");
        }

        var user = new User
        {
            Username = username,
            Email = email,
            PasswordHash = HashPassword(password)
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
        return GenerateJwtToken(user.Id);
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
    private string GenerateJwtToken(string userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, userId) }),
            Expires = DateTime.UtcNow.AddHours(3),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
