using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Marlin.sqlite.Data;
using Marlin.sqlite.Models;

namespace Marlin.sqlite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;

        public AuthController(IConfiguration configuration, DataContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginModel loginModel)
        {
            if (!IsValidLogin(loginModel.Username, loginModel.Password))
            {
                return Unauthorized();
            }

            var token = GenerateJwtToken(loginModel.Username);

            return Ok(new { Token = token });
        }

        private bool IsValidLogin(string username, string password)
        {
            
            var user = _context.Users.FirstOrDefault(u => u.Email == username);

            if (user == null || user.Password != password)
            {
                return false;
            }

            return true;
        }

        private string GenerateJwtToken(string username)
{
    var jwtConfig = _configuration.GetSection("JWT");
    var secretKey = Encoding.UTF8.GetBytes(jwtConfig["SecretKey"]); // Convert to bytes
    var issuer = jwtConfig["Issuer"];
    var audience = jwtConfig["Audience"];
    var expiryInMinutes = Convert.ToInt32(jwtConfig["ExpiryInMinutes"]);

    var securityKey = new SymmetricSecurityKey(secretKey); // Use the byte array
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    var claims = new[]
    {
        new Claim(ClaimTypes.Name, username)
    };

    var token = new JwtSecurityToken(
        issuer,
        audience,
        claims,
        expires: DateTime.UtcNow.AddMinutes(expiryInMinutes),
        signingCredentials: credentials
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
}

    }
}
