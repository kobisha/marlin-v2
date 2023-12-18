using Marlin.sqlite.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Linq;
using Marlin.sqlite.Models; 
using Microsoft.AspNetCore.Authorization;

namespace Marlin.sqlite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountDataFrontController : ControllerBase
    {
        private readonly DataContext _context;

        public AccountDataFrontController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAccountsData()
        {
            try
            {
                var query = @"
                    
                    SELECT
                        ac.""AccountID"",
                        ac.""LegalCode"",
                        ac.""Name"",
                        ac.""Description"",
                        ac.""Address"",
                        ac.""Phone"",
                        ac.""Email"",
                        case when ac.""Supplier"" = true then 1 else 0 end as ""IsVendor"",
                        case when ac.""Buyer"" = true then 1 else 0 end as ""IsRetail"",
                        Count(ca.""ProductID"") as ""ProductsCount""
                    FROM public.""Accounts"" as ac
                    LEFT JOIN public.""Catalogues"" as ca
                    ON ac.""AccountID"" = ca.""AccountID""
                    GROUP BY
                        ac.""AccountID"",
                        ac.""LegalCode"",
                        ac.""Name"",
                        ac.""Description"",
                        ac.""Address"",
                        ac.""Phone"",
                        ac.""Email"",
                        ac.""Supplier"",
                        ac.""Buyer""";

                var data = _context.Set<AccountData>().FromSqlRaw(query).ToList();

                var response = new
                {
                    Data = data
                };

                return Ok(response);
            }
            catch (Exception e)
            {
                return StatusCode(500, new { error = "An error occurred while processing the request.", details = e.Message });
            }
        }
    }
}
