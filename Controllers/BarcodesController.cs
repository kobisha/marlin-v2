using Marlin.sqlite.Data;
using Marlin.sqlite.Models;
using Marlin.sqlite.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Marlin.sqlite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BarcodesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUriService _uriService;

        public BarcodesController(DataContext context, IUriService uriService)
        {
            _context = context;
            _uriService = uriService;
        }

        [HttpPost("{AccountID}")]
        public IActionResult ImportData(string accountid, [FromBody] List<Barcodes> tableData)
        {
            try
            {
                foreach (var item in tableData)
                {
                    // Check if a record with the same Barcode exists in the database
                    var existingBarcode = _context.Barcodes.FirstOrDefault(b => b.Barcode == item.Barcode);

                    if (existingBarcode != null)
                    {
                        // Update the existing barcode's fields
                        existingBarcode.AccountId = accountid;
                        existingBarcode.ProductID = item.ProductID;
                        // You can update other fields here if needed
                    }
                    else
                    {
                        // Set the AccountId for the new barcode
                        item.AccountId = accountid;

                        // Add new barcode to the context
                        _context.Barcodes.Add(item);
                    }
                }

                // Save changes to the database
                _context.SaveChanges();

                return Ok(new { message = "Data imported/updated successfully" });
            }
            catch (Exception e)
            {
                return BadRequest(new { error = e.Message });
            }
        }
    }
}
