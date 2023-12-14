using Marlin.sqlite.Data;
using Marlin.sqlite.Models;
using Marlin.sqlite.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Marlin.sqlite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SLAByProductsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUriService _uriService;


        public SLAByProductsController(DataContext context, IUriService uriService)
        {
            _context = context;
            _uriService = uriService;


        }
        [HttpGet]
        public IActionResult GetData()
        {
            try
            {
                var query = @"
                    SELECT
                      MIN(""id"") as ""id"",
                    ""Product"" as ""Product"",

                    count(""OrderNumber"") as ""Orders"",

                    sum(""OrderedQuantity"") as ""OrderedQuantity"",  
                    sum(""OrderedAmount"") as ""OrderedAmount"", 

                    cast(avg(""SLAByQuantity"") as numeric(15, 2)) as ""SLAByQuantity"",

                    cast(avg(""SLAByAmount"") as numeric(15, 2)) as ""SLAByAmount"",

                    cast(avg(""SLAByQuantity"") as numeric(15, 2)) as ""InTimeOrders""

                FROM public.""ServiceLevels""

                group by ""Product""
                ";



                var data = _context.Set<SLAByProducts>().FromSqlRaw(query).ToList();

                var response = new
                {
                    Data = data
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
