using Marlin.sqlite.Data;
using Marlin.sqlite.Filter;
using Marlin.sqlite.Helper;
using Marlin.sqlite.Models;
using Marlin.sqlite.Services;
using Marlin.sqlite.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Marlin.sqlite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderDetailsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUriService _uriService;

        public OrderDetailsController(DataContext context, IUriService uriService)
        {
            _context = context;
            _uriService = uriService;
        }

        [HttpPost]

        public IActionResult CreateOrderDetails([FromBody] List<OrderDetails> orderDetails)
        {
            try
            {
                _context.OrderDetails.AddRange(orderDetails);
                _context.SaveChanges();

                return Ok(new { message = "Order details created successfully" });
            }
            catch (Exception e)
            {
                return BadRequest(new { error = e.Message });
            }
        }


        [HttpGet]
        public IActionResult GetData(int page = 1, int pageSize = 10)
        {
            try
            {
                var totalCount = _context.OrderDetails.Count();

                var data = _context.OrderDetails
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var formattedData = data.Select(d => new
                {

                    orderId = d.OrderHeaderID,
                    productID = d.Barcode,
                    unit = d.Unit,
                    quantity = d.Quantity,
                   price = d.Price,
                    Amount = d.Amount,
                    ReservedQuantity = d.ReservedQuantity

                    // Add more fields as necessary, following the same pattern
                });

                var response = new
                {
                    TotalCount = totalCount,
                    Page = page,
                    PageSize = pageSize,
                    Data = formattedData,
                    PreviousPage = page > 1 ? Url.Action("GetData", new { page = page - 1, pageSize }) : null,
                    NextPage = page < (totalCount + pageSize - 1) / pageSize ? Url.Action("GetData", new { page = page + 1, pageSize }) : null
                };

                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(new { error = e.Message });
            }
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetOrder(int id)
        {
            var header = await _context.OrderDetails.Where(a => a.Id == id).FirstOrDefaultAsync();
            if (header == null)
            {
                return BadRequest("User not found.");
            }
            return Ok(new Response<OrderDetails>(header));
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<OrderDetails>> DeleteDetals(int id)
        {
            var result = await _context.OrderDetails
            .FirstOrDefaultAsync(e => e.Id == id);
            if (result != null)
            {
                _context.OrderDetails.Remove(result);
                await _context.SaveChangesAsync();
                return result;
            }

            return null;
        }

        [HttpPut]
        public async Task<ActionResult<OrderDetails>> UpdateInvoice(OrderDetails item)
        {
            var result = await _context.OrderDetails
            .FirstOrDefaultAsync(e => e.Id == item.Id);

            if (result != null)
            {
                result.OrderHeaderID = item.OrderHeaderID;
                result.Barcode = item.Barcode;
                result.Unit = item.Unit;
                result.Quantity = item.Quantity;
                result.Price = item.Price;
                result.Amount = item.Amount;
                result.ReservedQuantity = item.ReservedQuantity;
               


                await _context.SaveChangesAsync();

                return result;
            }

            return null;
        }
    }
}
