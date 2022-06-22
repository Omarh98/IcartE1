using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IcartE1.Data;
using IcartE1.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;

namespace IcartE1.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Customer")]

    public class CustomerPaymentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ICipherService _cipherService;

        public CustomerPaymentsController(ApplicationDbContext context, ICipherService cipherService)
        {
            _context = context;
            _cipherService = cipherService;
        }

        // GET: api/CustomerPayments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerPayment>>> GetCustomerPayment()
        {
            var userId = User.FindFirstValue(claimType: ClaimTypes.NameIdentifier);
            var payments = await _context.CustomerPayment.Where(cp => cp.CustomerId == userId).AsNoTracking().ToListAsync();
            foreach (var payment in payments)
            {
                payment.CardNumber = _cipherService.Decrypt(payment.CardNumber);
            }
            return payments;
        }

        // POST: api/CustomerPayments
        [HttpPost]
        public async Task<ActionResult<CustomerPayment>> PostCustomerPayment([FromBody] CustomerPayment customerPayment)
        {
            customerPayment.CustomerId = User.FindFirstValue(claimType: ClaimTypes.NameIdentifier);
            if (ModelState.IsValid)
            {
                customerPayment.CardNumber = _cipherService.Encrypt(customerPayment.CardNumber);
               await _context.CustomerPayment.AddAsync(customerPayment);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetCustomerPayment", new { id = customerPayment.Id }, customerPayment);
            }
            return ValidationProblem();
        }

        // DELETE: api/CustomerPayments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerPayment(int id)
        {
            var customerPayment = await _context.CustomerPayment.FindAsync(id);
            if (customerPayment == null)
            {
                return NotFound(new {error="payment not found"});
            }

            _context.CustomerPayment.Remove(customerPayment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

   
    }
}
