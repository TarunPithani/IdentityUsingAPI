using IdentityUsingAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityUsingAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration config;

        public AuthService(UserManager<IdentityUser> userManager, IConfiguration config)
        {
            _userManager = userManager;
            this.config = config;
        }

        public async Task<bool> RegisterUser(LoginUser user)
        {
            var identityuser = new IdentityUser
            {
                UserName = user.Name,
                Email = user.Name
            };
            var result = await _userManager.CreateAsync(identityuser, user.Password);
            return result.Succeeded;
        }

        public async Task<bool> Login(LoginUser user)
        {
            var identityUser = await _userManager.FindByEmailAsync(user.Name);
            if (identityUser == null)
            {
                return false;
            }
            return await _userManager.CheckPasswordAsync(identityUser, user.Password);

        }

        public string GenerateTokenString(LoginUser user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Name),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("C1CF4B7DC4C4175B6618DE4F55CA4ABCDEFGH"));
            SigningCredentials signCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            SecurityToken securityToken = new JwtSecurityToken(
                issuer: config.GetSection("Jwt:Issuer").Value,
                claims: claims,
                expires: DateTime.Now.AddMinutes(3),
                audience: config.GetSection("Jwt:Audience").Value,
                signingCredentials: signCred
            );

            string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);
            Console.WriteLine(DateTime.Now);
            return tokenString;
        }
    }
}
