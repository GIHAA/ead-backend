using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TechFixBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        //[Authorize]
        [HttpPost("user/{id}/cart/add")]
        public async Task<IActionResult> AddToCart(string id, [FromBody] CartItemModel model)
        {
            try
            {
                await _authService.AddToCartAsync(id, model.ProductId, model.Quantity, model.Price);
                return Ok(new { Message = "Item added to cart" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        //[Authorize]
        [HttpDelete("user/{id}/cart/remove/{productId}")]
        public async Task<IActionResult> RemoveFromCart(string id, string productId)
        {
            try
            {
                await _authService.RemoveFromCartAsync(id, productId);
                return Ok(new { Message = "Item removed from cart" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        //[Authorize]
        [HttpPut("user/{id}/cart/update")]
        public async Task<IActionResult> UpdateCartItemQuantity(string id, [FromBody] CartItemModel model)
        {
            try
            {
                await _authService.UpdateCartItemQuantityAsync(id, model.ProductId, model.Quantity);
                return Ok(new { Message = "Cart item updated" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        //[Authorize]
        [HttpGet("user/{id}/cart")]
        public async Task<IActionResult> GetCart(string id)
        {
            try
            {
                var cartWithProducts = await _authService.GetCartAsync(id);
                return Ok(cartWithProducts);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


        // Restrict registration to Administrators only
        //[Authorize(Roles = "admin")]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            try
            {
                await _authService.RegisterAsync(model.Username, model.Email, model.Password, model.Role ?? "customer");
                return Ok(new { Message = "Registration successful" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // Allow login for all roles
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                // Call LoginAsync and get both token and user details
                var (token, user) = await _authService.LoginAsync(model.Email, model.Password);

                // Return both token and user in the response
                return Ok(new { Token = token, User = user });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
        }


        // Allow only CSR and Administrator to get all users
        [Authorize(Roles = "csr,admin")]
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers(int pageNumber = 1, int pageSize = 10)
        {
            try
            {

                if (pageNumber < 1)
                {
                    return BadRequest(new { Message = "Page number must be greater than 0." });
                }

                if (pageSize < 1)
                {
                    return BadRequest(new { Message = "Page size must be greater than 0." });
                }

                // Get users and total count
                var (pagedUsers, totalUsers) = await _authService.GetUsersAsync(pageNumber, pageSize);

                // Check if no users are found
                if (pagedUsers == null || !pagedUsers.Any())
                {
                    return NotFound(new { Message = "No users found." });
                }

                // Calculate total pages
                int totalPages = (int)Math.Ceiling((double)totalUsers / pageSize);

                // Ensure pageNumber does not exceed totalPages
                if (pageNumber > totalPages && totalPages > 0)
                {
                    return BadRequest(new { Message = "Page number exceeds total pages." });
                }

                var response = new
                {
                    TotalRecords = totalUsers,
                    TotalPages = totalPages,
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    Users = pagedUsers
                };

                return Ok(response);
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { Message = "An error occurred while retrieving users.", Details = ex.Message });
            }
        }

        // Only CSR and Admin can update user details
        [Authorize(Roles = "csr,admin")]
        [HttpPut("user/{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserUpdateModel updateModel)
        {
            try
            {
                await _authService.UpdateUserAsync(id, updateModel);
                return Ok(new { Message = "User updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        // Allow only Admin to delete a user
        [Authorize(Roles = "admin")]
        [HttpDelete("user/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                await _authService.DeleteUserAsync(id);
                return Ok(new { Message = "User deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // Deactivate the user's account (CSR/Admin)
        [Authorize(Roles = "csr,admin")]
        [HttpPut("user/{id}/deactivate")]
        public async Task<IActionResult> DeactivateAccount(string id)
        {
            try
            {
                await _authService.DeactivateAccountAsync(id);
                return Ok(new { Message = "Account deactivated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // Request account reactivation
        [Authorize]
        [HttpPost("user/{id}/request-reactivation")]
        public async Task<IActionResult> RequestReactivation(string id)
        {
            try
            {
                await _authService.RequestAccountReactivationAsync(id);
                return Ok(new { Message = "Reactivation request submitted" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // Approve account reactivation (CSR/Admin)
        [Authorize(Roles = "csr,admin")]
        [HttpPut("user/{id}/approve-reactivation")]
        public async Task<IActionResult> ApproveReactivation(string id)
        {
            try
            {
                await _authService.ApproveAccountReactivationAsync(id);
                return Ok(new { Message = "Account reactivated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        //get user by id
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            try
            {
                var user = await _authService.GetUserByIdAsync(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
