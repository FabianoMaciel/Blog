using Blog.Core.Models;
using Core.Handlers;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly IUserHandler _userHandler;

        public AuthController(IUserHandler userHandler)
        {
            _userHandler = userHandler;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserModel registerUser)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            string token = await _userHandler.Register(registerUser);
            if (!string.IsNullOrWhiteSpace(token))
                return Ok(token);

            return Problem("Error registering the user.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([Bind("Email, Password")] LoginModel login)
        {
            if (!ModelState.IsValid) { return ValidationProblem(ModelState); }

            string token = await _userHandler.Login(login);
            if (!string.IsNullOrWhiteSpace(token))
                return Ok(token);

            return Problem($"User or password incorrect.");
        }
    }
}
