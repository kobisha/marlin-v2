using Marlin.sqlite.Data;
using Marlin.sqlite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Linq;

namespace Marlin.sqlite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InvoiceDetailsFrontController : ControllerBase
    {
        private readonly DataContext _context;

        public InvoiceDetailsFrontController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("{InvoiceID}")]
        public IActionResult GetInvoiceDetails(string InvoiceID)
        {
            try
            {
                var query = @"
                    SELECT
    c.""Name"" AS ""Product"",
    id.""Barcode"" AS ""Barcode"",
    id.""Unit"",
    id.""Quantity"" AS ""InvoiceQuantity"",
    od.""Quantity"" AS ""OrderQuantity"",
    id.""Amount"" AS ""InvoiceAmount"",
    od.""Amount"" AS ""OrderAmount"",
    CASE WHEN od.""Amount"" - id.""Amount"" <> 0 THEN true ELSE false END AS ""RedStatus""
FROM public.""InvoiceDetails"" id
JOIN public.""InvoiceHeaders"" ih ON id.""InvoiceID"" = ih.""InvoiceID""
JOIN public.""OrderHeaders"" oh ON ih.""OrderID"" = oh.""OrderID""
JOIN public.""OrderDetails"" od ON od.""OrderHeaderID"" = ih.""OrderID"" AND id.""Barcode"" = od.""Barcode""
LEFT JOIN public.""Barcodes"" b ON id.""Barcode"" = b.""Barcode"" and oh.""AccountID"" = b.""AccountId""
LEFT JOIN public.""Catalogues"" c ON b.""ProductID"" = c.""ProductID""
WHERE id.""InvoiceID"" = @InvoiceID;";

                var parameters = new[]
                {
                    new NpgsqlParameter("@InvoiceID", InvoiceID)
                };

                var data = _context.Set<invdetails>().FromSqlRaw(query, parameters).ToList();

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
