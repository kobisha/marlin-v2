using Marlin.sqlite.Data;
using Marlin.sqlite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Marlin.sqlite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderStatusController : ControllerBase
    {
        private readonly DataContext _context;

        public OrderStatusController(DataContext context)
        {
            _context = context;
        }

        // Other methods (AddOrder, GetOrders, GetOrder, and DeleteStatus) are the same as your original code.

        [HttpPost("updateStatus")]
        public async Task<IActionResult> UpdateStatus([FromBody] OrderStatusUpdateDto updateDto)
        {
            try
            {
                var order = await _context.OrderHeaders.FirstOrDefaultAsync(o => o.OrderID == updateDto.OrderId);

                if (order == null)
                {
                    return NotFound("Order not found.");
                }

                order.StatusID = updateDto.StatusID;
                await _context.SaveChangesAsync();

                var orderStatusHistory = new OrderStatusHistory
                {
                    OrderID = updateDto.OrderId,
                    Date = DateTime.UtcNow,
                    StatusID = updateDto.StatusID,
                };
                _context.OrderStatusHistory.Add(orderStatusHistory);
                _context.SaveChanges();

                return Ok(new { message = "Status updated successfully ." });
            }
            catch (Exception e)
            {
                var errorMessage = "An error occurred while updating the SendStatus and StatusID.";
                if (e.InnerException != null)
                {
                    errorMessage += " Inner Exception: " + e.InnerException.Message;
                }

                return BadRequest(new { error = errorMessage });
            }
        }
    }
}
