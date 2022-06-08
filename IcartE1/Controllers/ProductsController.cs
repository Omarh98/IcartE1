using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IcartE1.Data;
using IcartE1.Models;
using AutoMapper;
using IcartE1.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace IcartE1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUploadService _uploadService;
      

        public ProductsController(ApplicationDbContext context, IMapper mapper
            , IUploadService uploadService)
        {
            _context = context;
            _mapper = mapper;
            _uploadService = uploadService;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.Include(p => p.ProductImages).ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.Include(p => p.ProductImages).FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public async Task<IActionResult> Create()
        {
            var vendors = await _context.Vendors.Select(v => new { v.Id, v.Name }).ToListAsync();
            var categories = await _context.Categories.Select(c => new { c.Id, c.Title }).ToListAsync();

            ViewBag.VendorId = new SelectList(vendors, "Id", "Name");
            ViewBag.CategoryId = new SelectList(categories, "Id", "Title");

            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] DerivedProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                var product = _mapper.Map<Product>(productViewModel);
                var productImages = new List<ProductImage>();

                foreach (var image in productViewModel.Images)
                    productImages.Add(new ProductImage { Url = await _uploadService.UploadFile("images/", image) });

                product.ProductImages = productImages;

                await _context.AddAsync(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productViewModel);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            var productViewModel = _mapper.Map<BaseProductViewModel>(product);

            if (product == null)
            {
                return NotFound();
            }

            var vendors = await _context.Vendors.Select(v => new { v.Id, v.Name }).ToListAsync();
            var categories = await _context.Categories.Select(c => new { c.Id, c.Title }).ToListAsync();

            ViewBag.VendorId = new SelectList(vendors, "Id", "Name");
            ViewBag.CategoryId = new SelectList(categories, "Id", "Title");

            return View(productViewModel);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] BaseProductViewModel productViewModel)
        {
            if (id != productViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var product = _mapper.Map<Product>(productViewModel);
                    var productImages = new List<ProductImage>();

                    if (productViewModel.Images != null)
                    {
                        var oldImages = await _context.ProductImages.Where(pi => pi.ProductId == id).ToListAsync();
                        oldImages.ForEach(img => _uploadService.DeleteFile(img.Url));

                        _context.RemoveRange(oldImages);

                        foreach (var image in productViewModel.Images)
                            productImages.Add(new ProductImage { Url = await _uploadService.UploadFile("images/", image) });

                        product.ProductImages = productImages;
                    }

                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(productViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(productViewModel);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.Include(p=>p.ProductImages).Where(p=>p.Id==id).FirstOrDefaultAsync();

            foreach (var image in product.ProductImages)
                _uploadService.DeleteFile(image.Url);

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
