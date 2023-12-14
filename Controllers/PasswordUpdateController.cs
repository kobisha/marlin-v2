using Marlin.sqlite.Data;
using Marlin.sqlite.Models;
using Marlin.sqlite.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Marlin.sqlite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PasswordUpdateController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUriService _uriService;

        public PasswordUpdateController(DataContext context, IUriService uriService)
        {
            _context = context;
            _uriService = uriService;
        }
        [HttpPut]
        public async Task<IActionResult> UpdatePassword(int userID, [FromBody] PasswordUpdateModel passwordUpdate)
        {
            // Find the user by UserID
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserID == userID);

            if (user == null)
            {
                return NotFound(); // User not found
            }

            // Verify the old password
            if (user.Password != passwordUpdate.OldPassword)
            {
                return BadRequest("Incorrect old password");
            }

            // Update the password with the new password
            user.Password = passwordUpdate.NewPassword;

            // Save changes to the database
            await _context.SaveChangesAsync();

            return Ok("Password updated successfully");
        }
    }
}
