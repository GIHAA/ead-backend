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
        public IActionResult Register([FromBody] RegisterModel model)
        {
            try
            {
                _authService.Register(model.Username, model.Email, model.Password, model.Role);
                return Ok(new { Message = "Registration successful" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            try
            {
                var token = _authService.Login(model.Email, model.Password);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
        }

        // GET: api/auth/users?pageNumber=1&pageSize=10
        [HttpGet("users")]
        public IActionResult GetUsers(int pageNumber = 1, int pageSize = 10)
        {
            var (pagedUsers, totalUsers) = _authService.GetUsers(pageNumber, pageSize);

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
        public IActionResult GetUser(string id)
        {
            var user = _authService.GetUserById(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // PUT: api/auth/user/{id}
        [HttpPut("user/{id}")]
        public IActionResult UpdateUser(string id, [FromBody] UserUpdateModel updateModel)
        {
            try
            {
                // log 
                Console.WriteLine("UpdateUser");
                // Retrieve the user from the database
                var existingUser = _authService.GetUserById(id);
                if (existingUser == null)
                    return NotFound(new { Message = "User not found" });

                // Update only the provided fields
                existingUser = _authService.UpdateUserFields(existingUser, updateModel);

                // Save the updated user
                _authService.UpdateUser(id, existingUser);
                return Ok(new { Message = "User updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // DELETE: api/auth/user/{id}
        [HttpDelete("user/{id}")]
        public IActionResult DeleteUser(string id)
        {
            try
            {
                _authService.DeleteUser(id);
                return Ok(new { Message = "User deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }

   
}
