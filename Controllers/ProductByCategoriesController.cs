using Marlin.sqlite.Data;
using Marlin.sqlite.Models;
using Marlin.sqlite.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Marlin.sqlite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductByCategoriesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUriService _uriService;

        public ProductByCategoriesController(DataContext context, IUriService uriService)
        {
            _context = context;
            _uriService = uriService;
        }

        [HttpPost("{AccountID}")]
        public IActionResult ImportData(string accountid, [FromBody] List<ProductsByCategories> tableData)
        {
            try
            {
                foreach (var item in tableData)
                {
                    // Check if a mapping with the same ProductID and CategoryID exists in the database
                    var existingMapping = _context.ProductsByCategories
                        .FirstOrDefault(m => m.ProductID == item.ProductID);

                    if (existingMapping != null)
                    {
                        // Update the existing mapping's fields
                        existingMapping.AccountID = accountid;
                        
                        existingMapping.CategoryID=item.CategoryID;
                    }
                    else
                    {
                        // Set the AccountID for the new mapping
                        item.AccountID = accountid;

                        // Add new mapping to the context
                        _context.ProductsByCategories.Add(item);
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
