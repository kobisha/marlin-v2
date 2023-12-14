using Marlin.sqlite.Data;
using Marlin.sqlite.Models;
using Marlin.sqlite.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Marlin.sqlite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RBFrontController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUriService _uriService;



        public RBFrontController(DataContext context, IUriService uriService)
        {
            _context = context;
            _uriService = uriService;


        }
        [HttpGet("{AccountID}/{SupplierID}")]
        public IActionResult GetData(string AccountID, string SupplierID)
        {
            try
            {
                var query = @"
            SELECT
    MIN(""Id"") as ""Id"",
    ""AccountID"",
    ""RetroBonusID"",
    ""DocumentNo"" ,
    ""SupplierID"",
    ""StartDate"",
    ""EndDate"",
    ""Status"",
    ""Condition"",
    ""PlanAmount"",
    greatest(""MinimalPercent"", ""PlanPercent"", ""ManufacturerPercent"") as ""RetroPercent""
FROM public.""RetroBonusHeaders""
where ""AccountID"" = @AccountID and ""SupplierID"" = @SupplierID
GROUP BY
    ""AccountID"",
    ""RetroBonusID"",
    ""DocumentNo"",
    ""SupplierID"",
    ""StartDate"",
    ""EndDate"",
    ""Status"",
    ""Condition"",
    ""PlanAmount"",
    greatest(""MinimalPercent"", ""PlanPercent"", ""ManufacturerPercent"")";

                var parameters = new[]
               {
                    new NpgsqlParameter("@AccountID", AccountID),
                    new NpgsqlParameter("@SupplierID", SupplierID)
                };

                var data = _context.Set<RBFront>().FromSqlRaw(query, parameters).ToList();

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
