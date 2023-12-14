using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Marlin.sqlite.Models;
using Dapper;
using Npgsql;
using k8s.Models;
using System.Xml.Linq;

namespace Marlin.sqlite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthFrontController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthFrontController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginModel loginModel)
        {
            var userInfo = GetUserInfoFromDatabase(loginModel.Username);

            if (userInfo == null)
            {
                return Unauthorized();
            }

            // Get the corresponding user from the Users model
            var user = GetUserFromDatabase(userInfo.Email);

            if (user == null || user.Password != loginModel.Password)
            {
                return Unauthorized();
            }

            var token = GenerateJwtToken(user,userInfo);

            return Ok(new { Token = token });
        }

        private UserInfo GetUserInfoFromDatabase(string email)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var query = @"
                    SELECT
                        U.""AccountID"",
                        U.""UserID"",
                        Acc.""Name"",
                        U.""FirstName"",
                        U.""LastName"",
                        U.""ContactNumber"",
                        U.""Email"",
                        U.""Description"",
                        U.""PositionInCompany"",
                        CASE WHEN Acc.""Buyer"" = true THEN 1 ELSE 0 END AS ""IsRetail""
                    FROM public.""Users"" as U
                    LEFT JOIN public.""Accounts"" as Acc
                        ON U.""AccountID"" = Acc.""AccountID""
                    WHERE TRIM(U.""Email"") = @Email;
                ";
                var userInfo = connection.QueryFirstOrDefault<UserInfo>(query, new { Email = email });
                return userInfo;
            }
        }

        private Users GetUserFromDatabase(string email)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM public.\"Users\" WHERE TRIM(\"Email\") = @Email;";
                var user = connection.QueryFirstOrDefault<Users>(query, new { Email = email });
                return user;
            }
        }

        private string GenerateJwtToken(Users user, UserInfo userInfo)
        {
            if (user == null || userInfo == null)
            {
                throw new ArgumentException("User or UserInfo is null.");
            }

            var jwtConfig = _configuration.GetSection("JWT");
            var secretKey = Encoding.UTF8.GetBytes(jwtConfig["SecretKey"]);
            var issuer = jwtConfig["Issuer"];
            var audience = jwtConfig["Audience"];
            var expiryInMinutes = Convert.ToInt32(jwtConfig["ExpiryInMinutes"]);

            var securityKey = new SymmetricSecurityKey(secretKey);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Email),
        new Claim("UserID", user.UserID.ToString()),
        new Claim("AccountName", userInfo.Name),
        new Claim("FirstName", user.FirstName),
        new Claim("LastName", user.LastName),
        new Claim("ContactNumber", user.ContactNumber),
        new Claim("Email", user.Email),
        new Claim("Description", user.Description),
        new Claim("PositionInCompany", user.PositionInCompany),
       new Claim("IsRetail", userInfo.IsRetail ? "1" : "0"),
       new Claim("AccountID", user.AccountID)


        // Add other user-related claims here
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
