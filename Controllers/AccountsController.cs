using Marlin.sqlite.Data;
using Marlin.sqlite.Filter;
using Marlin.sqlite.Helper;
using Marlin.sqlite.Models;
using Marlin.sqlite.Services;
using Marlin.sqlite.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System;

namespace Marlin.sqlite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUriService _uriService;
        private readonly ILogger<AccountsController> _logger;

        public AccountsController(DataContext context, IUriService uriService, ILogger<AccountsController> logger)
        {
            _context = context;
            _uriService = uriService;
            _logger = logger;
        }
        
        [HttpPost]
        public IActionResult CreateAccount(Accounts account)
        {
            try
            {
                _context.Accounts.Add(account);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, "An error occurred while saving the account.");
            }
        }
        
        [HttpGet]

        public async Task<IActionResult> GetUsers([FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _context.Accounts
               .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
               .Take(validFilter.PageSize)
               .ToListAsync();
            var totalRecords = await _context.Accounts.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<Accounts>(pagedData, validFilter, totalRecords, _uriService, route);
            return Ok(pagedReponse);
        }
        
        [HttpGet("{id}")]

        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _context.Accounts.Where(a => a.id == id).FirstOrDefaultAsync();
            if (user == null)
            {
                return BadRequest("User not found.");
            }
            return Ok(new Response<Accounts>(user));
        }
        
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Accounts>> DeleteAccount(int id)
        {
            var result = await _context.Accounts
            .FirstOrDefaultAsync(e => e.id == id);
            if (result != null)
            {
                _context.Accounts.Remove(result);
                await _context.SaveChangesAsync();
                return result;
            }

            return null;
        }
        
        [HttpPut]
        public async Task<ActionResult<Accounts>>UpdateUser(Accounts user)
        {
            var result = await _context.Accounts
            .FirstOrDefaultAsync(e => e.id == user.id);

            if (result != null)
            {
                result.AccountID = user.AccountID;
                result.LegalCode = user.LegalCode;
                result.Name = user.Name;
                result.Description = user.Description;
                result.Address = user.Address;
                result.Phone = user.Phone;
                result.Email = user.Email;
                result.Supplier = user.Supplier;
                result.Buyer = user.Buyer;

                await _context.SaveChangesAsync();

                return result;
            }

            return null;
        }
    }
}
