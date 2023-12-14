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
    public class SLAByOrdersController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUriService _uriService;


        public SLAByOrdersController(DataContext context, IUriService uriService)
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
                    ""OrderNumber"" as ""OrderNumber"",

                    ""OrderDate"" as ""OrderDate"",

                    ""Shop"" as ""Shop"",  

                    cast(avg(""SLAByQuantity"") as numeric(15, 2)) as ""SLAByQuantity"",

                    cast(avg(""SLAByAmount"") as numeric(15, 2)) as ""SLAByAmount"",

                    cast(avg(""SLAByQuantity"") as numeric(15, 2)) as ""InTimeOrders""

                FROM public.""ServiceLevels""

                group by ""OrderNumber"",""OrderDate"",
                ""Shop""
                ";



                var data = _context.Set<SLAByOrder>().FromSqlRaw(query).ToList();

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
