using Microsoft.AspNetCore.Mvc;
using TechFixSolution.AuthServices.Models;
using TechFixSolution.AuthServices.Services;

namespace TechFixSolution.AuthServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        // POST /api/auth/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest model)
        {
            var user = _authService.Authenticate(model.Username, model.Password);
            if (user == null)
            {
                return Unauthorized("Invalid credentials");
            }

            return Ok(new { UserId = user.Id, Username = user.Username, Role = user.Role });
        }

        // POST /api/auth/register
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest model)
        {
            var user = _authService.Register(model);
            if (user == null)
            {
                return BadRequest("Username already exists");
            }

            return Ok(new { UserId = user.Id, Username = user.Username, Role = user.Role });
        }

        // GET /api/auth/user/{id}
        [HttpGet("user/{id}")]
        public IActionResult GetUser(int id)
        {
            var user = _authService.GetUserById(id);
            if (user == null)
            {
                return NotFound("User not found");
            }
            return Ok(user);
        }

        // PUT /api/auth/user/{id}
        [HttpPut("user/{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UpdateUserRequest model)
        {
            var user = _authService.UpdateUser(id, model);
            if (user == null)
            {
                return NotFound("User not found");
            }
            return Ok(user);
        }

        // DELETE /api/auth/user/{id}
        [HttpDelete("user/{id}")]
        public IActionResult DeleteUser(int id)
        {
            var result = _authService.DeleteUser(id);
            if (!result)
            {
                return NotFound("User not found");
            }
            return Ok("User deleted successfully");
        }
    }
}
