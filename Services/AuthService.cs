using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TechFixBackend.Repository;
using TechFixBackend.Services;

public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly NotificationService _notificationService;
    private readonly string _key;
    private readonly IProductRepository _productRepository;

    public AuthService(IUserRepository userRepository, NotificationService notificationService, IProductRepository productRepository  ,  string key)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        _key = key ?? throw new ArgumentNullException(nameof(key));
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
    }

    // Register a new user
    public async Task RegisterAsync(string username , string email, string password, string role = "customer")
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            throw new ArgumentException("Email and password must be provided.");
        }

        var existingUser = await _userRepository.GetUserByEmailAsync(email);
        if (existingUser != null)
        {
            throw new Exception("Username or Email already exists");
        }

        var user = new User
        {
            Name = username,
            Email = email,
            PasswordHash = HashPassword(password),
            Role = role,
            AccountCreationDate = DateTime.UtcNow
        };

        await _userRepository.AddUserAsync(user);

        // Send a notification to the user about registration
        await SendNotificationSafely(user.Id, $"Welcome {user.Email}, your account has been successfully created.");
    }

    // Login an existing user
    public async Task<(string Token, User User)> LoginAsync(string email, string password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            throw new ArgumentException("Email and password must be provided.");
        }

        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null || !VerifyPassword(password, user.PasswordHash))
        {
            throw new Exception("Invalid username or password");
        }

        // Remove the password hash from the user object for security reasons
        user.PasswordHash = null;

        // Generate JWT token if login is successful
        var token = GenerateJwtToken(user.Id, user.Role);

        // Send a notification to the user about successful login
        await SendNotificationSafely(user.Id, "Login successful. Welcome back!");

        // Return both token and user details
        return (token, user);
    }

    // Modify user account details
    public async Task<bool> UpdateUserAsync(string userId, UserUpdateModel updateModel)
    {
        if (string.IsNullOrEmpty(userId))
        {
            throw new ArgumentException("User ID must be provided.");
        }

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

        // Send a notification to the user about the update
        await SendNotificationSafely(userId, "Your profile has been updated successfully.");

        return updated;
    }

    // Deactivate the user's account
    public async Task DeactivateAccountAsync(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            throw new ArgumentException("User ID must be provided.");
        }

        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        user.Status = "Deactivated";
        var updated = await _userRepository.UpdateUserAsync(userId, user);
        if (!updated)
        {
            throw new Exception("Account deactivation failed");
        }

        await SendNotificationSafely(userId, "Your account has been deactivated. Please contact support if you wish to reactivate it.");
    }

    // Request reactivation of the user's account
    public async Task RequestAccountReactivationAsync(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            throw new ArgumentException("User ID must be provided.");
        }

        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        if (user.Status != "Deactivated")
        {
            throw new Exception("Account is not deactivated. No reactivation required.");
        }

        // Notify the CSR/Administrator about the reactivation request
        await SendNotificationSafely(userId, "Your reactivation request has been submitted. Awaiting approval.");
    }

    // Approve reactivation of the user's account (handled by CSR/Administrator)
    public async Task ApproveAccountReactivationAsync(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            throw new ArgumentException("User ID must be provided.");
        }

        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        if (user.Status != "Deactivated")
        {
            throw new Exception("Account is not deactivated.");
        }

        user.Status = "Active";
        var updated = await _userRepository.UpdateUserAsync(userId, user);
        if (!updated)
        {
            throw new Exception("Account reactivation failed");
        }

        // Notify the user about account activation
        await SendNotificationSafely(userId, "Your account has been reactivated. You can now log in.");
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


        return existingUser;
    }

    // Safe notification sending method
    private async Task SendNotificationSafely(string userId, string message)
    {
        try
        {
            await _notificationService.SendNotificationToUserAsync(userId, message);
        }
        catch (Exception ex)
        {
            // Log the exception for debugging, but don't interrupt the main process
            Console.WriteLine($"Notification sending failed: {ex.Message}");
        }
    }

    // Get a paginated list of users
    public async Task<(List<User> users, long totalUsers)> GetUsersAsync(int pageNumber, int pageSize)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;

        // Fetch the list of users with pagination
        var users = await _userRepository.GetUsersAsync(pageNumber, pageSize);
        var totalUsers = await _userRepository.GetTotalUsersAsync();

        return (users, totalUsers);
    }

    // Delete a user by their ID
    public async Task<bool> DeleteUserAsync(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            throw new ArgumentException("User ID must be provided.");
        }

        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        var deleted = await _userRepository.DeleteUserAsync(userId);
        if (!deleted)
        {
            throw new Exception("User deletion failed");
        }


        await SendNotificationSafely(userId, "Your account has been deleted successfully.");

        return true;
    }

    public async Task<User> GetUserByIdAsync(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            throw new ArgumentException("User ID must be provided.");
        }

        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        return user;
    }


     public async Task AddToCartAsync(string userId, string productId, int quantity, double price)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        var existingItem = user.Cart.FirstOrDefault(item => item.ProductId == productId);
        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
        }
        else
        {
            user.Cart.Add(new CartItem
            {
                ProductId = productId,
                Quantity = quantity,
                Price = price
            });
        }

        // Update total price
        user.TotalPrice = user.Cart.Sum(item => item.Price * item.Quantity);

        // Update user
        await _userRepository.UpdateUserAsync(userId, user);
    }

    // Remove an item from the user's cart
    public async Task RemoveFromCartAsync(string userId, string productId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        var itemToRemove = user.Cart.FirstOrDefault(item => item.ProductId == productId);
        if (itemToRemove != null)
        {
            user.Cart.Remove(itemToRemove);

            // Update total price
            user.TotalPrice = user.Cart.Sum(item => item.Price * item.Quantity);

            // Update user
            await _userRepository.UpdateUserAsync(userId, user);
        }
    }

    // Update the quantity of an item in the user's cart
    public async Task UpdateCartItemQuantityAsync(string userId, string productId, int quantity)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        var existingItem = user.Cart.FirstOrDefault(item => item.ProductId == productId);
        if (existingItem != null)
        {
            existingItem.Quantity = quantity;

            // Update total price
            user.TotalPrice = user.Cart.Sum(item => item.Price * item.Quantity);

            // Update user
            await _userRepository.UpdateUserAsync(userId, user);
        }
    }

    // Get the user's cart
 public async Task<List<CartItemWithProduct>> GetCartAsync(string userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        var cartWithProducts = new List<CartItemWithProduct>();

        // Loop through each cart item and fetch product details
        foreach (var cartItem in user.Cart)
        {
            var product = await _productRepository.GetProductByIdAsync(cartItem.ProductId);
            if (product != null)
            {
                cartWithProducts.Add(new CartItemWithProduct
                {
                    ProductId = cartItem.ProductId,
                    ProductName = product.ProductName,
                    ProductDescription = product.ProductDescription,
                    Price = cartItem.Price,
                    Quantity = cartItem.Quantity,
                    ProductImageUrl = product.ProductImageUrl
                });
            }
        }

        return cartWithProducts;
    }
}
