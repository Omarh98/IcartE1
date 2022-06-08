using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IcartE1.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using IcartE1.Services;
using IcartE1.Models;

namespace IcartE1.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Customer")]

    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<IActionResult> GetOrdersAsync()
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return Ok(await _context.Orders.Where(o => o.CustomerId == customerId).Select(o => new { o.Id, o.DateOrdered, o.Total }).ToListAsync());
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderAsync(int id)
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var order = await _context.Orders.Where(o => o.Id == id && o.CustomerId == customerId).Include(o => o.OrderItems).FirstOrDefaultAsync();

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }


        // POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<Order>> AddOrderAsync([FromBody] AddOrderViewModel orderViewModel)
        {
            var cart = SessionHelper.GetObjectFromJson<Dictionary<string, CartItemViewModel>>(HttpContext.Session, "cart");

            if (cart == null || (!orderViewModel.IsCash && orderViewModel.PaymentId <= 0) || orderViewModel.BranchId <= 0) return ValidationProblem();

            var customer = await _context.Customers.FindAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var orderItems = cart.Values.ToList().Select(c => new OrderItem { ProductId = c.Product.Id, Quantity = c.Quantity }).ToList();
            float subTotal = cart.Values.Sum(i => i.Product.Price * i.Quantity);
            float discount = 0;

            if (orderViewModel.VoucherId != null)
            {
                var voucher = await _context.Vouchers.FindAsync(orderViewModel.VoucherId);

                if (voucher == null) return BadRequest("Invalid voucher");
                if (voucher.CustomerId != customer.Id) return BadRequest("User can't use this voucher");
                if (voucher.ExpiryDate.CompareTo(DateTime.Today) < 0) return BadRequest("Voucher Expired");
                if (voucher.MinOrder > subTotal) return BadRequest($"Add {voucher.MinOrder - subTotal} EGP to your order to be able to use this voucher");

                discount = voucher.Value<=subTotal?voucher.Value:0;
                customer.Vouchers.Remove(voucher);
            }

            if (!orderViewModel.IsOnline)
            {
                var batchIdList = cart.Values.ToList().Select(c => new { BatchId = c.BatchId, Quantity = c.Quantity });
                var batches = await _context.Batches.Where(b => batchIdList.Select(bi => bi.BatchId).ToList().Contains(b.Id)).ToListAsync();

                batches.ForEach(b =>
                {
                    b.Quantity -= batchIdList.Where(bi => bi.BatchId == b.Id).First().Quantity;
                });
            }
            else
            {
                var batches = await _context.Batches.Where(b => b.BranchId == orderViewModel.BranchId && b.Quantity > 0 &&
               orderItems.Select(oi => oi.ProductId).ToList().Contains(b.ProductId)).ToListAsync();

                var groups = batches.OrderBy(oi => oi.ArrivalDate).GroupBy(oi => oi.ProductId).ToList();

                orderItems.ForEach(oi =>
                {
                    var filteredBatches = groups.Where(g => g.Key == oi.ProductId).FirstOrDefault().ToList();
                    var batch = filteredBatches.Where(b => b.Quantity >= oi.Quantity).FirstOrDefault();

                    batch.Quantity -= oi.Quantity;
                    _context.Update(batch);
                });
            }

            var order = new Order
            {
                DateOrdered = DateTime.Now,
                IsCash = orderViewModel.IsCash,
                OrderItems = (ICollection<OrderItem>)orderItems,
                SubTotal = subTotal,
                Discount = discount,
                Total = subTotal - discount,
                BranchId = orderViewModel.BranchId,
                CustomerId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            };

            if (!orderViewModel.IsCash)
                if (await _context.CustomerPayment.AnyAsync(cp => cp.Id == orderViewModel.PaymentId && cp.CustomerId == customer.Id && cp.ExpiryDate.CompareTo(DateTime.Today) < 0))
                    order.CustomerPaymentId = orderViewModel.PaymentId;
                else return BadRequest("Invalid payment method");

            await _context.Orders.AddAsync(order);

            customer.RewardPoints += (int)Math.Round(order.Total / 10);

            if(customer.RewardPoints>= 500)
            {
                customer.Vouchers.Add(new Voucher {
                    Title = "Loyalty Voucher",
                    Description = "You've received a discount voucher for your loyalty",
                    ExpiryDate = DateTime.Today.AddDays(14),
                    CustomerId = customer.Id,
                    MinOrder = 100.0f,
                    Value = customer.RewardPoints/10
                });;

                customer.RewardPoints -= 500;
            }

            orderItems.ForEach(async oi =>
            {
                var salesEntity = await _context.Sales.FindAsync(oi.ProductId, orderViewModel.BranchId, DateTime.Today);
                if (salesEntity != null)
                    salesEntity.Quantity += oi.Quantity;
                else
                {
                    var salesItem = new Sales
                    {
                        BranchId = orderViewModel.BranchId,
                        Date = DateTime.Today,
                        ProductId = oi.ProductId,
                        Quantity = oi.Quantity
                    };

                    await _context.Sales.AddAsync(salesItem);
                }

            });

            await _context.SaveChangesAsync();

            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", null);

            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        }

    }
}
