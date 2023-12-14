using Marlin.sqlite.Data;
using Marlin.sqlite.Models;
using Marlin.sqlite.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Numerics;

namespace Marlin.sqlite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SLAByVendorsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUriService _uriService;


        public SLAByVendorsController(DataContext context, IUriService uriService)
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
                    ""Vendor"" as ""Vendor"",
                    ""OrderDate"" as ""OrderDate"",

                    count(""OrderNumber"") as ""Orders"",

                    sum(""OrderedAmount"") as ""Amount"",  

                    cast(avg(""SLAByQuantity"") as numeric(15, 2)) as ""SLAByQuantity"",

                    cast(avg(""SLAByAmount"") as numeric(15, 2)) as ""SLAByAmount"",

                    cast(avg(""SLAByQuantity"") as numeric(15, 2)) as ""InTimeOrders""

                FROM public.""ServiceLevels""

                group by ""Vendor"",""OrderDate""
                ";

        

                var data = _context.Set<SLAByVendors>().FromSqlRaw(query).ToList();

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
