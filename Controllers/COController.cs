﻿using Marlin.sqlite.Data;
using Marlin.sqlite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

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
                // Check if the relationship is approved
                var isApproved = _context.AccountRelations
                .Where(ar => ar.Account == User.Identity.Name && ar.ConnectedAccount == accountID && ar.Approved)
                 .Any();


                if (!isApproved)
                {
                    // If not approved, return a message
                    return BadRequest(new { error = "You are not connected to this account." });
                }

                // If approved, proceed to retrieve and process orders
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
                var errorMessage = "An error occurred while processing the request.";
                if (e.InnerException != null)
                {
                    errorMessage += " Inner Exception: " + e.InnerException.Message;
                }

                return BadRequest(new { error = errorMessage });
            }
        }
    }
}
