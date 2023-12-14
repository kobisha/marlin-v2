using Marlin.sqlite.Data;
using Marlin.sqlite.Filter;
using Marlin.sqlite.Wrappers;
using Marlin.sqlite.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Marlin.sqlite.Services;
using Marlin.sqlite.Helper;
using Microsoft.AspNetCore.Authorization;

namespace Marlin.sqlite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUriService _uriService;

        public UsersController(DataContext context, IUriService uriService)
        {
            _context = context;
            _uriService = uriService;
        }
        [HttpPost]
        public IActionResult CreateUsers(Users users)
        {
            try
            {
                // Check if any users exist in the database
                bool usersExist = _context.Users.Any();

                // Set the initial value of userID based on existing users
                if (usersExist)
                {
                    int maxUserID = _context.Users.Max(u => u.UserID);
                    users.UserID = maxUserID + 1;
                }
                else
                {
                    users.UserID = 1001; // Set the first userID to 1001
                }

                _context.Users.Add(users);
                _context.SaveChanges();

                return Ok(users);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error occurred while saving account to the database.");
                Console.WriteLine(ex.Message); // Log or print the exception message for debugging purposes
                return StatusCode(500, ex.Message);
            }
        }



        [HttpGet]

        public async Task<IActionResult> GetUsers([FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.Users
               .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
               .Take(validFilter.PageSize)
               .ToListAsync();
            var totalRecords = await _context.Users.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<Users>(pagedData, validFilter, totalRecords, _uriService, route);
            return Ok(pagedReponse);
        }

        

        
        [HttpPut("{userID}")]
        public IActionResult UpdateUser(int userID, [FromBody] Users updatedUser)
        {
            // Find the user by UserID
            var existingUser = _context.Users.FirstOrDefault(u => u.UserID == userID);

            if (existingUser == null)
            {
                return NotFound(); // User not found
            }

            // Update fields that are different from the original
            if (!string.IsNullOrWhiteSpace(updatedUser.FirstName))
            {
                existingUser.FirstName = updatedUser.FirstName;
            }

            if (!string.IsNullOrWhiteSpace(updatedUser.LastName))
            {
                existingUser.LastName = updatedUser.LastName;
            }

            if (!string.IsNullOrWhiteSpace(updatedUser.ContactNumber))
            {
                existingUser.ContactNumber = updatedUser.ContactNumber;
            }

            if (!string.IsNullOrWhiteSpace(updatedUser.Email))
            {
                existingUser.Email = updatedUser.Email;
            }

            if (!string.IsNullOrWhiteSpace(updatedUser.Description))
            {
                existingUser.Description = updatedUser.Description;
            }

            if (!string.IsNullOrWhiteSpace(updatedUser.PositionInCompany))
            {
                existingUser.PositionInCompany = updatedUser.PositionInCompany;
            }

            if (!string.IsNullOrWhiteSpace(updatedUser.Password))
            {
                existingUser.Password = updatedUser.Password;
            }

            // Save changes to the database
            _context.SaveChanges();

            return Ok(existingUser);
        }
    }
}
