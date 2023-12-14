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
                        // You can update other fields here if needed
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

        /*[HttpGet]

        public IActionResult GetData(int page = 1, int pageSize = 10)
        {
            try
            {
                var totalCount = _context.ProductCategories.Count();

                var data = _context.ProductCategories
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var formattedData = data.Select(d => new
                {

                    AccountID = d.AccountID,
                    categoryid = d.CategoryID,
                    parentFolder = d.ParentFolder,
                    code = d.Code,
                    name = d.Name


                    // Add more fields as necessary, following the same pattern
                });

                var response = new
                {
                    TotalCount = totalCount,
                    Page = page,
                    PageSize = pageSize,
                    Data = formattedData,
                    PreviousPage = page > 1 ? Url.Action("GetData", new { page = page - 1, pageSize }) : null,
                    NextPage = page < (totalCount + pageSize - 1) / pageSize ? Url.Action("GetData", new { page = page + 1, pageSize }) : null
                };

                return Ok(response);
            }*/
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
