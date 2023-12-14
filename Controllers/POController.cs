using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Marlin.sqlite.Data;
using Marlin.sqlite.JsonModels;
using Marlin.sqlite.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Marlin.sqlite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class POController : ControllerBase
    {
        private readonly DataContext _context;

        public POController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult CreateOrder([FromBody] List<OrderHeaders> orders)
        {
            try
            {
                foreach (var order in orders)
                {
                    var existingOrder = _context.OrderHeaders.Include(o => o.Products).FirstOrDefault(o => o.OrderID == order.OrderID);

                    if (existingOrder == null)
                    {
                        // Order does not exist, create a new one
                        var orderHeadersData = new OrderHeaders
                        {
                            AccountID = order.AccountID,
                            OrderID = order.OrderID,
                            Date = order.Date.ToUniversalTime(),
                            Number = order.Number,
                            SenderID = order.SenderID,
                            ReceiverID = order.ReceiverID,
                            ShopID = order.ShopID,
                            Amount = order.Amount,
                            StatusID = 1,
                            SendStatus = 1,
                            Products = new List<OrderDetails>() // Initialize the Products collection
                        };

                        foreach (var product in order.Products)
                        {
                            var orderDetailsData = new OrderDetails
                            {
                                OrderHeaderID = orderHeadersData.OrderID,
                                Barcode = product.Barcode,
                                Unit = product.Unit,
                                Quantity = product.Quantity,
                                Price = product.Price,
                                Amount = product.Amount,
                                ReservedQuantity = 0
                            };
                            orderHeadersData.Products.Add(orderDetailsData);
                        }

                        _context.OrderHeaders.Add(orderHeadersData);
                    }
                    else
                    {
                        // ...
                    }
                }

                _context.SaveChanges();
                return Ok(new { message = "Orders created/updated successfully" });
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
