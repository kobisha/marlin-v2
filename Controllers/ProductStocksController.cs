using Marlin.sqlite.Data;
using Marlin.sqlite.Models;
using Marlin.sqlite.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Marlin.sqlite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductStocksController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUriService _uriService;

        public ProductStocksController(DataContext context, IUriService uriService)
        {
            _context = context;
            _uriService = uriService;
        }


        [HttpPost]
        public IActionResult ImportData([FromBody] List<StocksOfProducts> tableData)
        {
            try
            {
                _context.ProductsStocks.AddRange(tableData);
                _context.SaveChanges();

                return Ok(new { message = "Data imported successfully" });
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
                var totalCount = _context.ProductsStocks.Count();

                var data = _context.ProductsStocks
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var formattedData = data.Select(d => new
                {

                    AccountId = d.AccountID,
                    ProductID = d.Barcode,
                    ShopID = d.ShopID,
                    Quantity = d.Quantity,



                    
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
    }
}
