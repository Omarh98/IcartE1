using IcartE1.Data;
using IcartE1.Models;
using IcartE1.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IcartE1.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Customer")]

    public class CartController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/cart
        [HttpGet]
        public IActionResult GetCart()
        {
            var cart = SessionHelper.GetObjectFromJson<Dictionary<string, CartItemViewModel>>(HttpContext.Session, "cart");

            if (cart == null)
                return NotFound();

            return Ok(cart);

        }

        // POST: api/cart
        [HttpPost]
        public async Task<IActionResult> AddCartItemAsync([FromBody] AddCartItemViewModel addCartItemViewModel)
        {
            if (addCartItemViewModel.ProductId > 0 && addCartItemViewModel.BatchId >= 0 && addCartItemViewModel.Quantity > 0)
            {
                
                Product product;
                if (addCartItemViewModel.BatchId > 0)
                {
                    product = await _context.Batches.Where(b => b.Id == addCartItemViewModel.BatchId && b.ProductId == addCartItemViewModel.ProductId).Include(b => b.Product).Select(b=>b.Product).FirstOrDefaultAsync();
                    if (product == null)
                        return NotFound();

                    
                    product.Batches = null;
                }
                else
                {
                    product = await _context.Products.FindAsync(addCartItemViewModel.ProductId);
                    if (product == null)
                        return NotFound();
                }



                var cart = SessionHelper.GetObjectFromJson<Dictionary<string, CartItemViewModel>>(HttpContext.Session, "cart");
                if (cart == null)
                    cart = new Dictionary<string, CartItemViewModel>();
                var key = (product.Id.ToString() + addCartItemViewModel.BatchId.ToString());

                CartItemViewModel item;
                if (cart.TryGetValue(key, out item))
                {
                    item.Quantity += addCartItemViewModel.Quantity;

                    cart[key] = item;
                }
                else
                {
                    item = new CartItemViewModel
                    {
                        Product = product,
                        BatchId = addCartItemViewModel.BatchId,
                        Quantity = addCartItemViewModel.Quantity
                    };

                    cart.Add(key, item);
                }

                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);

                return CreatedAtAction("GetCart", cart);
            }
            return BadRequest();

        }

        // DELETE: api/cart/10
        [HttpDelete("{key}")]
        public IActionResult DeleteCartItem(string key)
        {
            var cart = SessionHelper.GetObjectFromJson<Dictionary<string, CartItemViewModel>>(HttpContext.Session, "cart");

            if (!cart.ContainsKey(key)) return NotFound();

            if (cart[key].Quantity == 1) cart.Remove(key);
            else cart[key].Quantity--;

            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return Ok();
        }
        [HttpDelete]
        public IActionResult DeleteCart()
        {
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", null);
            return NoContent();
        }


        private Dictionary<string, CartItemViewModel> AddCartItem(Product product, int batchId, int quantity)
        {
            var cart = SessionHelper.GetObjectFromJson<Dictionary<string, CartItemViewModel>>(HttpContext.Session, "cart");
            if (cart == null)
                cart = new Dictionary<string, CartItemViewModel>();
            var key = (product.Id.ToString() + batchId.ToString());

            CartItemViewModel item;
            if (cart.TryGetValue(key, out item))
            {
                item.Quantity += quantity;

                cart[key] = item;
            }
            else
            {
                item = new CartItemViewModel
                {
                    Product = product,
                    BatchId = batchId,
                    Quantity = quantity
                };

                cart.Add(key, item);
            }

            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return cart;
        }
    }
}
