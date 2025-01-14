using Azure.Core;
using Final_Descent.Services;
using Microsoft.AspNetCore.Mvc;

namespace Final_Descent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;

        public AuthController(JwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost("generate-token")]
        public IActionResult GenerateToken([FromBody] TokenRequest tokenRequest)
        {
            if (string.IsNullOrEmpty(tokenRequest.Email) || string.IsNullOrEmpty(tokenRequest.Role))
            {
                return BadRequest(new { Message = "Email and role are required." });
            }

            if (tokenRequest.Role != "Admin" && tokenRequest.Role != "SuperAdmin")
            {
                return BadRequest("Invalid role. Allowed roles are 'Admin' and 'SuperAdmin'.");
            }

            var token = _jwtService.GenerateToken(tokenRequest.Email, tokenRequest.Role);
            return Ok(new { Token = token });
        }

        public class TokenRequest
        {
            public string Email { get; set; }
            public string Role { get; set; }
        }
    }
}
