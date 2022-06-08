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

    public class ListItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ListItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

  

        // POST: api/ListItems
        [HttpPost]
        public async Task<ActionResult<ListItem>> PostListItem([FromBody] ListItem listItem)
        {
            if (ModelState.IsValid)
            {
                var oldItem = await _context.ListItems.FindAsync(listItem.ListId, listItem.ProductId);

                if (oldItem == null)
                    await _context.ListItems.AddAsync(listItem);
                else
                    oldItem.Quantity += listItem.Quantity;

               await _context.SaveChangesAsync();
               return StatusCode(201);
            }
            return ValidationProblem();
            
        }

        // DELETE: api/ListItems/5
        [HttpDelete]
        public async Task<IActionResult> DeleteListItem([FromQuery]int listId,[FromQuery]int productId)
        {
            var listItem = await _context.ListItems.FindAsync(listId,productId);
            if (listItem == null)
            {
                return NotFound();
            }

            _context.ListItems.Remove(listItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        
    }
}
