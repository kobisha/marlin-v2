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
    public class CataloguesController : ControllerBase
    {
        private readonly DataContext _context;

        public CataloguesController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("{AccountID}")]
        public IActionResult ImportData(string accountid, [FromBody] List<Catalogues> tableData)
        {
            try
            {
                foreach (var item in tableData)
                {
                    // Check if the product with the same ProductID exists in the database
                    var existingProduct = _context.Catalogues.FirstOrDefault(p => p.ProductID == item.ProductID);

                    if (existingProduct != null)
                    {
                        // Update existing product's fields
                        existingProduct.SourceCode = item.SourceCode;
                        existingProduct.Name = item.Name;
                        existingProduct.Description = item.Description;
                        existingProduct.Unit = item.Unit;
                        existingProduct.Status = item.Status;
                        existingProduct.LastChangeDate = DateTime.UtcNow;
                    }
                    else
                    {
                        // Set the AccountID for the new product
                        item.AccountID = accountid;

                        // Add new product to the context
                        _context.Catalogues.Add(item);
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
