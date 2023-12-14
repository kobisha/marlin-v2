using Marlin.sqlite.Data;
using Marlin.sqlite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Marlin.sqlite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderSendStatusController : ControllerBase
    {

        private readonly DataContext _context;

        public OrderSendStatusController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult UpdateSendStatus([FromBody] SendStatusUpdateDto sendStatusUpdateDto)
        {
            try
            {
                var orderToUpdate = _context.OrderHeaders.FirstOrDefault(o => o.OrderID == sendStatusUpdateDto.OrderId);
                if (orderToUpdate != null)
                {
                    var oldStatusId = orderToUpdate.StatusID;
                    orderToUpdate.SendStatus = sendStatusUpdateDto.SendStatus ? 1 : 2;
                    if (sendStatusUpdateDto.SendStatus)
                    {
                        
                        orderToUpdate.SendStatus = 3;
                    }
                    _context.SaveChanges();


                    var orderStatusHistory = new OrderStatusHistory
                    {
                        OrderID = sendStatusUpdateDto.OrderId,
                        Date = DateTime.UtcNow,
                        StatusID = orderToUpdate.StatusID,
                    };
                    _context.OrderStatusHistory.Add(orderStatusHistory);
                    _context.SaveChanges();

                    return Ok(new { message = "SendStatus and StatusID updated successfully" });
                }
                else
                {
                    return BadRequest(new { error = "Order not found" });
                }
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
