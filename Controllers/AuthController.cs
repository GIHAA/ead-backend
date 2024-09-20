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

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            try
            {
                await _authService.RegisterAsync(model.Email, model.Password, model.Role);
                return Ok(new { Message = "Registration successful" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            
            try
            {
                var token = await _authService.LoginAsync(model.Email, model.Password);
                return Ok(new { Token = token });
                
            }
            catch (Exception ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
        }

        // GET: api/auth/users?pageNumber=1&pageSize=10
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers(int pageNumber = 1, int pageSize = 10)
        {
            var (pagedUsers, totalUsers) = await _authService.GetUsersAsync(pageNumber, pageSize);

            var response = new
            {
                TotalRecords = totalUsers,
                TotalPages = (int)Math.Ceiling((double)totalUsers / pageSize),
                CurrentPage = pageNumber,
                PageSize = pageSize,
                Users = pagedUsers
            };

            return Ok(response);
        }

        // GET: api/auth/user/{id}
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            try
            {
                var user = await _authService.GetUserByIdAsync(id);
                if (user == null)
                    return NotFound(new { Message = "User not found" });

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // PUT: api/auth/user/{id}
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
                return BadRequest(new { Message = ex.Message });
            }
        }

        // DELETE: api/auth/user/{id}
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
    }
}
