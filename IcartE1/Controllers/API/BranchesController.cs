using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IcartE1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using IcartE1.Helpers;
using IcartE1.Models;

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
        public async Task<IActionResult> GetBranches([FromBody] BranchFilterViewModel filterViewModel)
        {
            if (!ModelState.IsValid) return ValidationProblem();

            var customer = await _context.Customers.FindAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if(customer == null) return NotFound(new { error = "Client doesn't exist" });

            var branches = await _context.Branches.AsNoTracking().ToListAsync();
            var comparer = new DistanceComparer(filterViewModel.IsOnline?customer.Latitude:filterViewModel.latitude, filterViewModel.IsOnline ? customer.Longitude:filterViewModel.longitude);
            branches.Sort(comparer);

            var closeBranches = branches.Select(b => new
            {
                b.Id,
                b.Title,
                Distance = comparer.GetDistance(b.Latitude, b.Longitude, customer.Latitude, customer.Longitude)
            })
                .Where(b => b.Distance < (filterViewModel.IsOnline? 8000:500));

            if(branches.Any()) return Ok(closeBranches);
            return NotFound(new { error = "Out of service range"});
        }

    }
}
