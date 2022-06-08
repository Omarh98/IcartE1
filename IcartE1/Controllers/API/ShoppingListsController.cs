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
using System.Security.Claims;

namespace IcartE1.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Customer")]

    public class ShoppingListsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ShoppingListsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ShoppingLists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShoppingList>>> GetShoppingLists()
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _context.ShoppingLists.Where(sl=>sl.CustomerId==customerId).AsNoTracking().ToListAsync();
        }

        // GET: api/ShoppingLists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ShoppingList>> GetShoppingList(int id)
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var shoppingList = await _context.ShoppingLists.Where(sl => sl.CustomerId == customerId && sl.Id==id).Include(sl => sl.ListItems).ThenInclude(li => li.Product).AsNoTracking().FirstAsync();

            if (shoppingList == null)
            {
                return NotFound();
            }

            return shoppingList;
        }


        // POST: api/ShoppingLists
        [HttpPost]
        public async Task<ActionResult<ShoppingList>> PostShoppingList([FromQuery] string title)
        {
            if (string.IsNullOrEmpty(title))
                return ValidationProblem(title: "Title cannot be empty");

            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var shoppingList = new ShoppingList()
            {
                Title = title,
                DateCreated = DateTime.Today,
                CustomerId = customerId
            };
           await _context.ShoppingLists.AddAsync(shoppingList);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetShoppingList", new { id = shoppingList.Id }, shoppingList);
        }

        // DELETE: api/ShoppingLists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShoppingList(int id)
        {
            var shoppingList = await _context.ShoppingLists.Where(sl => sl.Id==id).Include(sl=>sl.ListItems).FirstAsync();
            if (shoppingList == null)
            {
                return NotFound();
            }
            _context.ListItems.RemoveRange(shoppingList.ListItems);
            _context.ShoppingLists.Remove(shoppingList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        
    }
}
