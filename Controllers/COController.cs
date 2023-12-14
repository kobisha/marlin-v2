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
    public class COController : ControllerBase
    {
        
        private readonly DataContext _context;

        public COController(DataContext context)
        {
            _context = context;
        }



        [HttpGet]
        public IActionResult GetOrders(string accountID)
        {
            try
            {
                var orders = _context.OrderHeaders
                    .Where(o => o.SenderID == accountID && (o.SendStatus == 1 || o.SendStatus == 2))
                    .Select(o => new
                    {
                        Id = o.Id,
                        AccountID = o.AccountID,
                        OrderID = o.OrderID,
                        Date = o.Date,
                        Number = o.Number,
                        SenderID = o.SenderID,
                        ReceiverID = o.ReceiverID,
                        ShopID = o.ShopID,
                        Amount = o.Amount,
                        StatusID = o.StatusID,
                        SendStatus = o.SendStatus,
                        Products = _context.OrderDetails
                            .Where(d => d.OrderHeaderID == o.OrderID)
                            .Select(p => new OrderDetails
                            {
                                Id = p.Id,
                                OrderHeaderID = p.OrderHeaderID,
                                Barcode = p.Barcode,
                                Unit = p.Unit,
                                Quantity = p.Quantity,
                                Price = p.Price,
                                Amount = p.Amount,
                                ReservedQuantity = p.ReservedQuantity,
                                
                            }).ToList()
                    })
                    .ToList();

                foreach (var order in orders)
                {
                    var orderToUpdate = _context.OrderHeaders.FirstOrDefault(o => o.OrderID == order.OrderID);
                    if (orderToUpdate != null)
                    {
                        orderToUpdate.SendStatus = 2;
                        orderToUpdate.StatusID = 2;
                    }

                   /* foreach (var product in order.Products)
                    {
                        var barcode = _context.Barcodes.FirstOrDefault(b => b.ProductID == product.ProductID)?.Barcode;
                        product.Barcode = barcode ?? ""; // Assign the barcode value to the temporary variable
                    }*/

                    var orderStatusHistory = new OrderStatusHistory
                    {
                        OrderID = orderToUpdate.OrderID,
                        Date = DateTime.UtcNow,
                        StatusID = orderToUpdate.StatusID
                    };
                    _context.OrderStatusHistory.Add(orderStatusHistory);
                }

                _context.SaveChanges();

                return Ok(orders);
            }
            catch (Exception e)
            {
                var errorMessage = "An error occurred while saving the entity changes.";
                if (e.InnerException != null)
                {
                    errorMessage += " Inner Exception: " + e.InnerException.Message;
                }

                return BadRequest(new { error = errorMessage });
            }
        }









    }
}

