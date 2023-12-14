using System;
using System.Linq;
using Marlin.sqlite.Data;
using Marlin.sqlite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Marlin.sqlite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StatusResultFrontController : ControllerBase
    {
        private readonly DataContext _context;

        public StatusResultFrontController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("{orderId}")]
        public IActionResult GetOrderStatus(string orderId)
        {
            try
            {
                var query = @"
                    SELECT osh.""StatusID"" as ""ID"", osh.""OrderID"", max(osh.""Date"") as ""Date"", os.""StatusName""
                    FROM public.""OrderStatusHistory"" as osh
                    JOIN public.""OrderStatus"" as os ON osh.""StatusID"" = os.""StatusID""
                    WHERE osh.""OrderID"" = @OrderID
                    group by osh.""StatusID"", osh.""OrderID"", os.""StatusName""
                    order by osh.""StatusID""";

                var parameters = new[]
{
    new Npgsql.NpgsqlParameter("@OrderID", orderId)
};

                var orderStatus = _context.Set<OrderStatusResult>().FromSqlRaw(query, parameters).ToList();

                var response = new
                {
                    Data = orderStatus
                };

                return Ok(new { message = "Status updated successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
