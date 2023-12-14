using Marlin.sqlite.Data;
using Marlin.sqlite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Marlin.sqlite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CatalogueFrontController : ControllerBase
    {
        private readonly DataContext _context;

        public CatalogueFrontController(DataContext context)
        {
            _context = context;
        }
        
        [HttpGet("{AccountID}/{CategoryID}")]
        public IActionResult GetData(string AccountID, string CategoryID)
        {
            try
            {
                var query = @"
                  WITH tmpPriceList AS (

    SELECT pl.""RetailerID"" AS ""RetailerID"", pl.""Barcode"" AS ""Barcode"", MAX(pl.""Date"") AS ""Date""

    FROM public.""PriceList"" AS pl

    JOIN public.""Barcodes"" as b ON pl.""Barcode"" = b.""Barcode"" AND pl.""RetailerID"" = b.""AccountId""

    JOIN public.""ProductsByCategories"" pbc ON pl.""Barcode"" = b.""Barcode"" AND pl.""RetailerID"" = pbc.""AccountID"" AND pbc.""CategoryID"" = @CategoryID

    WHERE pl.""RetailerID"" = @AccountID

    GROUP BY pl.""RetailerID"", pl.""Barcode""

    )                           

SELECT

    c.""Id"" AS ""Id"",

    c.""AccountID"" AS ""AccountID"", -- Include the Id column here

    b.""Barcode"" AS ""Barcode"",

    c.""ProductID"" AS ""ProductID"",

    c.""Name"" AS ""Product"",

    c.""Unit"" AS ""Unit"",

    c.""Status"" AS ""Status"",

    plf.""Price"" AS ""Price"",

    plf.""LastPrice"" AS ""LastPrice"",

    plf.""LastChangeDate"" AS ""LastChangeDate""

FROM public.""Catalogues"" c

JOIN public.""Barcodes"" b ON c.""ProductID"" = b.""ProductID"" AND c.""AccountID"" = b.""AccountId""

JOIN public.""ProductsByCategories"" pbc ON c.""ProductID"" = pbc.""ProductID"" AND c.""AccountID"" = pbc.""AccountID"" AND pbc.""CategoryID"" = @CategoryID

LEFT JOIN (SELECT  pl.""Barcode"" as ""Barcode"", pl.""RetailerID"" as ""RetailerID"", pl.""Price"" AS ""Price"", pl.""LastPrice"" AS ""LastPrice"", pl.""Date"" AS ""LastChangeDate""

			FROM public.""PriceList"" pl

			JOIN tmpPriceList tpl ON pl.""Barcode"" = tpl.""Barcode"" AND pl.""RetailerID"" = tpl.""RetailerID"" AND pl.""Date"" = tpl.""Date"") as plf

	 ON b.""Barcode"" = plf.""Barcode"" AND c.""AccountID"" = plf.""RetailerID""		

WHERE c.""AccountID"" = @AccountID";

                var parameters = new[]
                {
                    new Npgsql.NpgsqlParameter("@CategoryID", CategoryID),
                    new Npgsql.NpgsqlParameter("@AccountID", AccountID)
                };

                var data = _context.Set<temTable>().FromSqlRaw(query, parameters).ToList();

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
