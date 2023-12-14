using Marlin.sqlite.Data;
using Marlin.sqlite.Filter;
using Marlin.sqlite.Wrappers;
using Marlin.sqlite.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Marlin.sqlite.Services;
using Marlin.sqlite.Helper;
using Newtonsoft.Json.Linq;

using Microsoft.AspNetCore.Authorization;

namespace Marlin.sqlite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class ShopsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUriService _uriService;

        public ShopsController(DataContext context, IUriService uriService)
        {
            _context = context;
            _uriService = uriService;
        }

        [HttpPost]
        public IActionResult ImportData([FromBody] List<Shops> tableData)
        {
            try
            {
                foreach (var shopData in tableData)
                {
                    var existingShop = _context.Shops.FirstOrDefault(s => s.ShopID == shopData.ShopID);

                    if (existingShop != null)
                    {
                        // Update the existing shop record
                        existingShop.AccountID = shopData.AccountID;
                        existingShop.SourceCode = shopData.SourceCode;
                        existingShop.Name = shopData.Name;
                        existingShop.Description = shopData.Description;
                        existingShop.Address = shopData.Address;
                        existingShop.ContactPerson = shopData.ContactPerson;
                        existingShop.ContactNumber = shopData.ContactNumber;
                        existingShop.Email = shopData.Email;
                        existingShop.Region = shopData.Region;
                        existingShop.Format = shopData.Format;
                        existingShop.GPS = shopData.GPS;

                        _context.SaveChanges();
                    }
                    else
                    {
                        // Create a new shop record
                        _context.Shops.Add(shopData);
                        _context.SaveChanges();
                    }
                }

                return Ok(new { message = "Data imported successfully" });
            }
            catch (Exception e)
            {
                return BadRequest(new { error = e.Message });
            }
        }


        /*  [HttpGet]

          public IActionResult GetData(int page = 1, int pageSize = 10)
          {
              try
              {
                  var totalCount = _context.Shops.Count();

                  var data = _context.Shops
                      .Skip((page - 1) * pageSize)
                      .Take(pageSize)
                      .ToList();

                  var formattedData = data.Select(d => new
                  {

                      AccountID = d.AccountID,
                      ShopID = d.ShopID,
                      SourceCode = d.SourceCode,
                      Name = d.Name,
                      Description = d.Description,
                      Address = d.Address,
                      ContactPerson = d.ContactPerson,
                      ContactNumber = d.ContactNumber,
                      Email = d.Email,
                      Region = d.Region,
                      Format = d.Format,
                      GPS = d.GPS

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
              }
              catch (Exception e)
              {
                  return BadRequest(new { error = e.Message });
              }
          }*/

        [HttpGet]

        public async Task<IActionResult> GetShops(string AccountID)
        {
            var shops = await _context.Shops.Where(a => a.AccountID == AccountID).ToListAsync();

            if (shops == null || !shops.Any())
            {
                return BadRequest("No shops found for the given AccountID.");
            }

            return Ok(shops);
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Shops>> DeleteShops(string id)
        {
            var result = await _context.Shops
            .FirstOrDefaultAsync(e => e.AccountID == id);
            if (result != null)
            {
                _context.Shops.Remove(result);
                await _context.SaveChangesAsync();
                return result;
            }

            return null;
        }
        [HttpPut]
        public async Task<ActionResult<Shops>> UpdateShops(Shops item)
        {
            var result = await _context.Shops
            .FirstOrDefaultAsync(e => e.AccountID == item.AccountID);

            if (result != null)
            {
                result.AccountID = item.AccountID;
                result.ShopID = item.ShopID;
                result.SourceCode = item.SourceCode;
                result.Name = item.Name;
                result.Description = item.Description;
                result.Address = item.Address;
                result.ContactPerson = item.ContactPerson;
                result.ContactNumber = item.ContactNumber;
                result.Email = item.Email;
                result.Region = item.Region;
                result.Format = item.Format;
                result.GPS = item.GPS;




                await _context.SaveChangesAsync();

                return result;
            }

            return null;
        }
    }
}
