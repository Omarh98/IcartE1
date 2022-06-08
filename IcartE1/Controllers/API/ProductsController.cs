﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IcartE1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using IcartE1.Models;

namespace IcartE1.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Customer")]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] int categoryId, [FromQuery] int branchId)
        {
            if (categoryId > 0 && branchId > 0)
            {
                var products = await _context.Batches.Include(b => b.Product).ThenInclude(p => p.ProductImages)
               .Where(b => b.BranchId == branchId && b.Product.CategoryId == categoryId && b.Quantity > 0)
               .Select(b => new { b.ProductId, b.Product.Title, b.Product.Price, b.Product.ProductImages, b.Quantity }).ToListAsync();

                if (products == null)
                    return NotFound();

                var grouped = products.GroupBy(b => b.ProductId)
                    .Select(g => new
                    {
                        Id = g.Key,
                        Title = g.First().Title,
                        Price = g.First().Price,
                        ImageUrl = g.First().ProductImages.First().Url,
                        Quantity = g.Sum(b => b.Quantity)
                    }).ToList();


                return Ok(grouped);
            }

            return BadRequest();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductDetails(int id)
        {
            var product = await _context.Products.AsNoTracking().Where(p => p.Id == id).Include(p => p.ProductImages).FirstAsync();

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }
  

    }
}