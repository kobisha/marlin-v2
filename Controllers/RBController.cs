using Marlin.sqlite.Data;
using Marlin.sqlite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marlin.sqlite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RBController : ControllerBase
    {
        private readonly DataContext _context;

        public RBController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> PostRetroBonus([FromBody] List<RetroBonusHeader> retroBonuses)
        {
            try
            {
                foreach (var retroBonus in retroBonuses)
                {
                    // Check if a record with the same RetroBonusID exists in the database
                    var existingRetroBonus = await _context.RetroBonusHeaders
                        .FirstOrDefaultAsync(rb => rb.RetroBonusID == retroBonus.RetroBonusID);

                    if (existingRetroBonus == null)
                    {
                        // If the record with the specified RetroBonusID doesn't exist, add a new one
                        _context.RetroBonusHeaders.Add(retroBonus);
                    }
                    else
                    {
                        // If the record with the specified RetroBonusID exists, update its non-key properties
                        // Exclude the Id property from modification
                        _context.Entry(existingRetroBonus).State = EntityState.Detached;
                        retroBonus.Id = existingRetroBonus.Id; // Preserve the existing Id
                        _context.Update(retroBonus);
                    }

                    // Update related entities (RetroBonusDetails, RetroBonusPlanRanges) if needed
                    // You may need to handle related entities as well, depending on your data structure
                }

                // Commit changes to the database
                await _context.SaveChangesAsync();

                return Ok(new { message = "Data successfully saved to the database." });
                
            }
            catch (DbUpdateException ex)
            {
                // Handle exceptions and provide a meaningful error response
                // (similar to your existing error handling code)
                Console.WriteLine($"Error occurred while saving data: {ex.Message}");

                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }

                return StatusCode(500, ex.Message);
            }
        }




    }
}
