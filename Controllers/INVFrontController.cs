using Marlin.sqlite.Data;
using Marlin.sqlite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Globalization;
using System.Linq;

namespace Marlin.sqlite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class INVFrontController : ControllerBase
    {
        private readonly DataContext _context;

        public INVFrontController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("{AccountID}")]
        public IActionResult GetData(string AccountID)
        {
            try
            {
                var query = @"
            SELECT
                
                oh.""AccountID"",
                ih.""OrderID"",
                ih.""InvoiceID"",
                ih.""Date"", -- Assuming Date is stored as text
                ih.""WaybillNumber"",
                oh.""Number"" as ""OrderNumber"",
                ih.""Number"" as ""InvoiceNumber"",
                oh.""ReceiverID"" AS ""VendorID"",
                ac.""Name""  AS ""Vendor"",
                s.""Name"" AS ""Shop"",
                CAST(ih.""Amount"" AS decimal) as ""InvoiceAmount"",
                CAST(oh.""Amount"" AS decimal) as ""OrderAmount""
            FROM public.""OrderHeaders"" oh
            JOIN public.""InvoiceHeaders"" ih on oh.""OrderID"" = ih.""OrderID"" and oh.""AccountID"" = @AccountID
            LEFT JOIN public.""Shops"" s ON oh.""ShopID"" = s.""ShopID""
            LEFT JOIN public.""OrderStatus"" st ON oh.""StatusID"" = st.""Id""
            LEFT JOIN public.""Accounts"" ac ON oh.""ReceiverID""  = ac.""AccountID"";";


                var parameters = new[]
                {
                    new NpgsqlParameter("@AccountID", AccountID)
                };

                var data = _context.Set<invoicfront>().FromSqlRaw(query, parameters).ToList();

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
