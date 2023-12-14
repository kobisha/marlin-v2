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
    public class OrderHeaderController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUriService _uriService;

        public OrderHeaderController(DataContext context, IUriService uriService)
        {
           _context = context;
            _uriService = uriService;
        }

        [HttpPost]

        public IActionResult CreateOrderHeaders([FromBody] List<OrderHeaders> orderHeaders)
        {
            try
            {
                _context.OrderHeaders.AddRange(orderHeaders);
                _context.SaveChanges();

                return Ok(new { message = "Order headers created successfully" });
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
                var totalCount = _context.OrderHeaders.Count();

                var data = _context.OrderHeaders
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var formattedData = data.Select(d => new
                {

                    AccountID = d.AccountID,
                    OrderID = d.OrderID,
                    Date = d.Date,
                    Number = d.Number,
                    SenderID = d.SenderID,
                    ReceiverID = d.ReceiverID,
                    ShopID = d.ShopID,

                    Amount = d.Amount,
                    StatusID = d.StatusID
                    

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
            var header = await _context.OrderHeaders.Where(a => a.Id == id).FirstOrDefaultAsync();
            if (header == null)
            {
                return BadRequest("User not found.");
            }
            return Ok(new Response<OrderHeaders>(header));
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<OrderHeaders>> DeleteDetals(int id)
        {
            var result = await _context.OrderHeaders
            .FirstOrDefaultAsync(e => e.Id == id);
            if (result != null)
            {
                _context.OrderHeaders.Remove(result);
                await _context.SaveChangesAsync();
                return result;
            }

            return null;
        }

        [HttpPut]
        public async Task<ActionResult<OrderHeaders>> UpdateInvoice(OrderHeaders item)
        {
            var result = await _context.OrderHeaders
            .FirstOrDefaultAsync(e => e.Id == item.Id);

            if (result != null)
            {
                result.AccountID = item.AccountID;
                result.OrderID = item.OrderID;
                
                result.Date = item.Date;
                result.Number = item.Number;
                result.SenderID = item.SenderID;
                result.ReceiverID = item.ReceiverID;
                result.ShopID = item.ShopID;
                result.Amount = item.Amount;
                result.StatusID = item.StatusID;



                await _context.SaveChangesAsync();

                return result;
            }

            return null;
        }
    }
}
