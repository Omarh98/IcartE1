using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IcartE1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace IcartE1.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Customer")]

    public class BranchesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BranchesController(ApplicationDbContext context)
        {
            _context = context;
        }

        //[HttpGet("Branches")]
        //public async Task<IActionResult> GetBranches()
        //{
        //    var branches = await _context.Branches.Select(b => new { b.Id, b.Title }).AsNoTracking().ToListAsync();

        //    return Ok(branches);
        //}

        // GET: api/Branches
        [HttpGet]
        public async Task<ActionResult<Branch>> GetBranch([FromQuery] double latitude, [FromQuery] double longitude)
        {
            var branches = await _context.Branches.AsNoTracking().ToListAsync();
            var closestBranch = branches.ElementAt(0);
            double distance = double.MaxValue;
            double newDistance = 0;

            foreach (var branch in branches)
            {
                newDistance = GetDistance(branch.Longitude, branch.Latitude, longitude, latitude);

                if (newDistance < distance)
                    closestBranch = branch;
            }

            if (newDistance <= 1000)
                return Ok(closestBranch);
            else
                return NotFound();

        }

        private static double GetDistance(double longitude, double latitude, double otherLongitude, double otherLatitude)
        {
            var d1 = latitude * (Math.PI / 180.0);
            var num1 = longitude * (Math.PI / 180.0);
            var d2 = otherLatitude * (Math.PI / 180.0);
            var num2 = otherLongitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }
    }
}
