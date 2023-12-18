using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Marlin.sqlite.Data;

namespace Marlin.sqlite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RetroBonusConditionController : ControllerBase
    {
        private readonly DataContext _context;

        public RetroBonusConditionController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("{retroBonusId}")]
        public IActionResult GetConditionsForRetroBonus(string retroBonusId)
        {
            try
            {
                var query = @"
                    SELECT
                        rbr.""Id"",
                        rbr.""RangeName"",
                        rbr.""RangePercent""
                    FROM public.""RetroBonusPlanRanges"" rbr
                    WHERE rbr.""RetroBonusID"" = @RetroBonusID
                    ORDER BY rbr.""RangePercent"" ASC";

                var parameters = new[]
                {
                    new Npgsql.NpgsqlParameter("@RetroBonusID", retroBonusId)
                };

                var ranges = _context.retroBonusConditionFronts
                    .FromSqlRaw(query, parameters).ToList();

                var response = new
                {
                    Data = ranges
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
