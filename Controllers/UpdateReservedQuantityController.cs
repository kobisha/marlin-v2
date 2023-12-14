using Marlin.sqlite.Data;
using Marlin.sqlite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Marlin.sqlite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UpdateReservedQuantityController : ControllerBase
    {
        private readonly DataContext _context;

        public UpdateReservedQuantityController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("orderdetails")]
        public async Task<IActionResult> UpdateReservedQuantity([FromBody] List<OrderDetailsUpdate> orderDetailsUpdates)
        {
            foreach (var orderDetailsUpdate in orderDetailsUpdates)
            {
                var existingOrderDetails = await _context.OrderDetails.FirstOrDefaultAsync(o => o.OrderHeaderID == orderDetailsUpdate.OrderID && o.Barcode == orderDetailsUpdate.Barcode);

                if (existingOrderDetails != null)
                {
                    // Convert ReservedQuantity from int to decimal
                    existingOrderDetails.ReservedQuantity = (decimal)orderDetailsUpdate.ReservedQuantity;
                }
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = "Data successfully updated in the database." });
        }
    }
}
