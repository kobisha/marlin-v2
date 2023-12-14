using Marlin.sqlite.Data;
using Marlin.sqlite.Models;
using Microsoft.AspNetCore.Authorization;
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
    public class RetroBonusDetsilsFrontController : ControllerBase
    {
        private readonly DataContext _context;

        public RetroBonusDetsilsFrontController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("{ShopID}/{RetroBonusID}")]
        public IActionResult GetData(string ShopID, string RetroBonusID)
        {
            try
            {
                var query = @"
                   WITH tmpPurchaseOrders AS (

                        SELECT OH.""AccountID"", IH.""Date"", ID.""Barcode"" , ID.""Amount""

                        FROM public.""InvoiceHeaders"" as IH

                        JOIN public.""OrderHeaders"" as OH

                            ON IH.""OrderID"" = OH.""OrderID"" and OH.""ShopID"" = @ShopID

                        JOIN public.""InvoiceDetails"" as ID

                            ON IH.""InvoiceID"" = ID.""InvoiceID""     

                    )

                    SELECT
                        

                        RBD.""RetroBonusID"",

                        RBD.""Barcode"",

                        C.""Name"" as ""Product"",

                        max(case when GREATEST(RBD.""MinimalPercent"", RBD.""PlanPercent"", RBD.""ManufacturerPercent"") is null

                        	then 0 else GREATEST(RBD.""MinimalPercent"", RBD.""PlanPercent"", RBD.""ManufacturerPercent"") end)  as ""RetroPercent"",

                        SUM(case when PUR.""Amount"" is null then 0 else PUR.""Amount"" end) as ""PurchaseAmount"",

                        MAX(case when PS.""Quantity"" is null then 0 else PS.""Quantity"" end) as ""Stock""

                    FROM public.""RetroBonusDetails"" as RBD

                    JOIN public.""RetroBonusHeaders"" as RB

                        ON RBD.""RetroBonusID"" = RB.""RetroBonusID""

                    LEFT JOIN public.""Barcodes"" as B

                        ON RBD.""Barcode"" = B.""Barcode""

                    LEFT JOIN public.""Catalogues"" as C

                        ON C.""ProductID"" = B.""ProductID""

                    LEFT JOIN tmpPurchaseOrders as PUR

                        ON RBD.""Barcode"" = PUR.""Barcode"" AND RB.""AccountID"" = PUR.""AccountID""

                            AND PUR.""Date"" BETWEEN RB.""StartDate"" AND (CASE WHEN RB.""EndDate"" IS NULL THEN NOW() ELSE RB.""EndDate"" END)

                    LEFT JOIN public.""ProductsStocks"" as PS

                        ON RBD.""Barcode"" = PS.""Barcode"" AND RB.""AccountID"" = PS.""AccountID"" and PS.""ShopID"" = @ShopID

                    WHERE RBD.""RetroBonusID"" = @RetroBonusID

                  GROUP BY  RBD.""RetroBonusID"", RBD.""Barcode"", C.""Name""";

                var parameters = new[]
                {
                    new NpgsqlParameter("@ShopID", ShopID),
                    new NpgsqlParameter("@RetroBonusID", RetroBonusID)
                };


                var data = _context.RetroBonusResults.FromSqlRaw(query, parameters).ToList();

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
