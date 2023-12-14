using Marlin.sqlite.Data;
using Marlin.sqlite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Marlin.sqlite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderDetailsFrontController : ControllerBase
    {
        private readonly DataContext _context;

        public OrderDetailsFrontController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("{OrderID}")]
        public IActionResult GetData(string OrderID)
        {
            try
            {
                var query = @"
            SELECT

    

    c.""Name"" AS ""Product"",

    od.""Barcode"" AS ""Barcode"",

    od.""Unit"",

    od.""Quantity"",

    od.""Price"",

    od.""Amount"",

    od.""ReservedQuantity"",

	id.""Quantity"" as ""InvoiceQuantity"",

    id.""Amount"" as ""InvoiceAmount"",

	Case when id.""Quantity"" > 0

		then case when od.""Quantity""- id.""Quantity"">0 then true else false end

		else case when od.""Quantity""- od.""ReservedQuantity"">0 then true else false end

		end as ""RedStatus""

FROM public.""OrderDetails"" od

LEFT JOIN public.""Barcodes"" b ON od.""Barcode"" = b.""Barcode""

LEFT JOIN public.""Catalogues"" c ON b.""ProductID"" = c.""ProductID""

LEFT JOIN public.""InvoiceHeaders"" ih ON od.""OrderHeaderID"" = ih.""OrderID""

LEFT JOIN public.""InvoiceDetails"" id ON id.""InvoiceID"" = ih.""InvoiceID"" and id.""Barcode"" = od.""Barcode""

WHERE od.""OrderHeaderID"" = @OrderID";

                var parameters = new[]
                {
            new Npgsql.NpgsqlParameter("@OrderID", OrderID)
        };

                var data = _context.Set<OrderDetailsFront>().FromSqlRaw(query, parameters).ToList();

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
