using Marlin.sqlite.Data;
using Marlin.sqlite.Models;
using Marlin.sqlite.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Marlin.sqlite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductCategoriesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUriService _uriService;

        public ProductCategoriesController(DataContext context, IUriService uriService)
        {
            _context = context;
            _uriService = uriService;
        }

        [HttpPost("{AccountID}")]
        public IActionResult ImportData(string accountid, [FromBody] List<ProductCategories> tableData)
        {
            try
            {
                foreach (var item in tableData)
                {
                    // Check if a record with the same CategoryID exists in the database
                    var existingCategory = _context.ProductCategories.FirstOrDefault(c => c.CategoryID == item.CategoryID);

                    if (existingCategory != null)
                    {
                        // Update the existing category's fields
                        existingCategory.AccountID = accountid;
                        existingCategory.ParentFolder = item.ParentFolder;
                        existingCategory.Code = item.Code;
                        existingCategory.Name = item.Name;
                        
                    }
                    else
                    {
                        // Set the AccountID for the new category
                        item.AccountID = accountid;

                        // Add new category to the context
                        _context.ProductCategories.Add(item);
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

       
        [HttpGet]

        public async Task<IActionResult> GetShops(string AccountID)
        {
            var products = await _context.ProductCategories.Where(a => a.AccountID == AccountID).ToListAsync();

            if (products == null || !products.Any())
            {
                return BadRequest("No categories found for the given AccountID.");
            }

            return Ok(products);


        }
    }
}
