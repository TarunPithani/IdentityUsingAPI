using IdentityUsingAPI.Models;
using IdentityUsingAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdentityUsingAPI.Controllers
{
    public class AuthToken
    {
        public string token { get; set; }
        public DateTime tokenExpiry { get; set; }
        public DateTime tokenGenerated { get; set; }

    }
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IAuthService service;

        public EmployeeController(IAuthService service)
        {
            this.service = service;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(LoginUser user)
        {
            if (await service.RegisterUser(user))
            {
                return Ok("Successfully Register");
            }
            return BadRequest("Something went wrong");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUser user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (await service.Login(user))
            {
                var tokenString = service.GenerateTokenString(user);
                var token = new AuthToken
                {
                    token = tokenString,
                    tokenGenerated = DateTime.Now,
                    tokenExpiry = DateTime.Now.AddMinutes(3)
                };
                return Ok(token);
            }
            return BadRequest();
        }
    }
}
