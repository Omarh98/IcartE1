using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IcartE1.Data;
using Microsoft.AspNetCore.Authorization;
using IcartE1.Services;
using IcartE1.Models;

namespace IcartE1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BatchesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IQrService _qrService;
        private readonly IUploadService _uploadService;

        public BatchesController(ApplicationDbContext context, IQrService qrService, IUploadService uploadService)
        {
            _context = context;
            _qrService = qrService;
            _uploadService = uploadService;
        }

        // GET: Batches
        public async Task<IActionResult> Index([FromQuery] int productId, [FromQuery] int branchId, [FromQuery] int warehouseId)
        {
            ViewBag.ProductId = new SelectList(await _context.Products.AsNoTracking().ToListAsync(), "Id", "Title");
            ViewBag.BranchId = new SelectList(await _context.Branches.AsNoTracking().ToListAsync(), "Id", "Title");
            ViewBag.WarehouseId = new SelectList(await _context.Warehouses.AsNoTracking().ToListAsync(), "Id", "Title");
            var model = new BatchFilterViewModel
            {
                ProductId = productId,
                BranchId = branchId,
                WarehouseId = warehouseId
            };
            if (productId == 0 && branchId == 0 && warehouseId == 0)
            {
                var batches = await _context.Batches.Where(b => b.Quantity > 0 && b.ExpiryDate > DateTime.Today)
                    .Include(b => b.Branch).Include(b => b.Product).Include(b => b.Warehouse).AsNoTracking().ToListAsync();
                model.Batches=batches;
                return View(model);
            }
            else if (productId == 0 && branchId == 0 && warehouseId != 0)
            {

                var batches = await _context.Batches.Where(b => b.Quantity > 0 && b.ExpiryDate > DateTime.Today && b.WarehouseId == warehouseId).Include(b => b.Branch).Include(b => b.Product).Include(b => b.Warehouse).ToListAsync();
                model.Batches=batches;
                return View(model);
            }
            else if (productId == 0 && branchId != 0 && warehouseId == 0)
            {
                var batches = await _context.Batches.Where(b => b.Quantity > 0 && b.ExpiryDate > DateTime.Today && b.BranchId == branchId).Include(b => b.Branch).Include(b => b.Product).Include(b => b.Warehouse).ToListAsync();
                model.Batches = batches;
                return View(model);
            }
            else if (productId == 0 && branchId != 0 && warehouseId != 0)
            {
                var batches = await _context.Batches.Where(b => b.Quantity > 0 && b.ExpiryDate > DateTime.Today && (b.BranchId == branchId || b.WarehouseId == warehouseId)).Include(b => b.Branch).Include(b => b.Product).Include(b => b.Warehouse).ToListAsync();
                model.Batches = batches;
                return View(model);
            }
            else if (productId != 0 && branchId == 0 && warehouseId == 0)
            {
                var batches = await _context.Batches.Where(b => b.Quantity > 0 && b.ExpiryDate > DateTime.Today && b.ProductId == productId).Include(b => b.Branch).Include(b => b.Product).Include(b => b.Warehouse).ToListAsync();
                model.Batches = batches;
                return View(model);

            }
            else if (productId != 0 && branchId == 0 && warehouseId != 0)
            {
                var batches = await _context.Batches.Where(b => b.Quantity > 0 && b.ExpiryDate > DateTime.Today && b.ProductId == productId && b.WarehouseId == warehouseId).Include(b => b.Branch).Include(b => b.Product).Include(b => b.Warehouse).ToListAsync();
                model.Batches = batches;
                return View(model);
            }
            else if (productId != 0 && branchId != 0 && warehouseId == 0)
            {
                var batches = await _context.Batches.Where(b => b.Quantity > 0 && b.ExpiryDate > DateTime.Today && b.ProductId == productId && b.BranchId == branchId).Include(b => b.Branch).Include(b => b.Product).Include(b => b.Warehouse).ToListAsync();
                model.Batches = batches;
                return View(model);
            }
            else if (productId != 0 && branchId != 0 && warehouseId != 0)
            {
                var batches = await _context.Batches.Where(b => b.Quantity > 0 && b.ExpiryDate > DateTime.Today && b.ProductId == productId && (b.BranchId == branchId || b.WarehouseId == warehouseId)).Include(b => b.Branch).Include(b => b.Product).Include(b => b.Warehouse).ToListAsync();
                model.Batches = batches;
                return View(model);
            }

            return BadRequest();
        }

        public async Task<IActionResult> Products()
        {
            var batches =await _context.Batches.Where(b => b.ExpiryDate > DateTime.Now)
                .Include(b => b.Warehouse).Include(b => b.Branch).Include(b => b.Product).ThenInclude(p => p.Vendor).ToListAsync();

               var groupedBatches=batches.GroupBy(b => new { b.ProductId ,b.BranchId,b.WarehouseId});

            var productsStocks = groupedBatches.Select(g => new ProductStocksViewModel
            {
                Key = g.Key.ProductId,
                Quantity = g.Sum(b => b.Quantity),
                ReorderQuantity=g.FirstOrDefault().Product.ReorderQuantity,
                Email = g.FirstOrDefault().Product.Vendor.Email,
                PhoneNumber = g.FirstOrDefault().Product.Vendor.PhoneNumber,
                Title = g.FirstOrDefault().Product.Title,
                WarehouseTitle=g.FirstOrDefault().Warehouse==null?"": g.FirstOrDefault().Warehouse.Title,
                BranchTitle = g.FirstOrDefault().Branch == null ? "" : g.FirstOrDefault().Branch.Title,
                WarehouseId= (int)(g.FirstOrDefault().WarehouseId == null ? 0 : g.FirstOrDefault().WarehouseId),
                BranchId = (int)(g.FirstOrDefault().BranchId == null ? 0 : g.FirstOrDefault().BranchId),
            }).OrderBy(ps => ps.Quantity);


            return View(productsStocks);

        }

        // GET: Batches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var batch = await _context.Batches
                .Include(b => b.Branch)
                .Include(b => b.Product)
                .Include(b => b.Warehouse)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (batch == null)
            {
                return NotFound();
            }

            return View(batch);
        }

        // GET: Batches/Create
        public async Task<IActionResult> CreateAsync()
        {
            var branches = await _context.Branches.Select(b => new { b.Id, b.Title }).AsNoTracking().ToListAsync();
            var warehouses = await _context.Warehouses.Select(w => new { w.Id, w.Title }).AsNoTracking().ToListAsync();
            branches.Insert(0, new { Id = 0, Title = "None" });
            warehouses.Insert(0, new { Id = 0, Title = "None" });

            ViewData["BranchId"] = new SelectList(branches, "Id", "Title");
            ViewData["WarehouseId"] = new SelectList(warehouses, "Id", "Title");
            ViewData["ProductId"] = new SelectList(await _context.Products.Select(p => new { p.Id, p.Title }).AsNoTracking().ToListAsync(), "Id", "Title");
            return View();
        }

        // POST: Batches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Batch batch)
        {
            if (ModelState.IsValid)
            {
                if (batch.WarehouseId != 0 && batch.BranchId != 0 || batch.WarehouseId == 0 && batch.BranchId == 0)
                {
                    ModelState.AddModelError("", "Batch can only be present in a warehouse or a branch");
                    var _branches = await _context.Branches.Select(b => new { b.Id, b.Title }).AsNoTracking().ToListAsync();
                    var _warehouses = await _context.Warehouses.Select(w => new { w.Id, w.Title }).AsNoTracking().ToListAsync();
                    _branches.Insert(0, new { Id = 0, Title = "None" });
                    _warehouses.Insert(0, new { Id = 0, Title = "None" });

                    ViewData["BranchId"] = new SelectList(_branches, "Id", "Title");
                    ViewData["WarehouseId"] = new SelectList(_warehouses, "Id", "Title");
                    ViewData["ProductId"] = new SelectList(await _context.Products.Select(p => new { p.Id, p.Title }).AsNoTracking().ToListAsync(), "Id", "Title");
                    return View(batch);
                }
                if (batch.BranchId == 0)
                    batch.BranchId = null;
                else
                    batch.WarehouseId = null;

                _context.Add(batch);
                await _context.SaveChangesAsync();

                string qrText = String.Format("batchId:{0}\nproductId:{1}", batch.Id, batch.ProductId);
                batch.QrImageUrl = await _qrService.CreateQRCodeAsync(qrText);

                _context.Batches.Update(batch);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            var branches = await _context.Branches.Select(b => new { b.Id, b.Title }).AsNoTracking().ToListAsync();
            var warehouses = await _context.Warehouses.Select(w => new { w.Id, w.Title }).AsNoTracking().ToListAsync();
            branches.Insert(0, new { Id = 0, Title = "None" });
            warehouses.Insert(0, new { Id = 0, Title = "None" });

            ViewData["BranchId"] = new SelectList(branches, "Id", "Title");
            ViewData["WarehouseId"] = new SelectList(warehouses, "Id", "Title");
            ViewData["ProductId"] = new SelectList(await _context.Products.Select(p => new { p.Id, p.Title }).AsNoTracking().ToListAsync(), "Id", "Title");
            return View(batch);
        }

        // GET: Batches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var batch = await _context.Batches.FindAsync(id);
            if (batch == null)
            {
                return NotFound();
            }
            var branches = await _context.Branches.Select(b => new { b.Id, b.Title }).AsNoTracking().ToListAsync();
            var warehouses = await _context.Warehouses.Select(w => new { w.Id, w.Title }).AsNoTracking().ToListAsync();
            branches.Insert(0, new { Id = 0, Title = "None" });
            warehouses.Insert(0, new { Id = 0, Title = "None" });

            ViewData["BranchId"] = new SelectList(branches, "Id", "Title");
            ViewData["WarehouseId"] = new SelectList(warehouses, "Id", "Title");
            ViewData["ProductId"] = new SelectList(await _context.Products.Select(p => new { p.Id, p.Title }).AsNoTracking().ToListAsync(), "Id", "Title");
            return View(batch);
        }

        // POST: Batches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromForm] Batch batch)
        {
            if (id != batch.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (batch.WarehouseId != 0 && batch.BranchId != 0)
                {
                    ModelState.AddModelError("", "Batch can only be present in a warehouse or a branch");
                    var _branches = await _context.Branches.Select(b => new { b.Id, b.Title }).AsNoTracking().ToListAsync();
                    var _warehouses = await _context.Warehouses.Select(w => new { w.Id, w.Title }).AsNoTracking().ToListAsync();
                    _branches.Insert(0, new { Id = 0, Title = "None" });
                    _warehouses.Insert(0, new { Id = 0, Title = "None" });

                    ViewData["BranchId"] = new SelectList(_branches, "Id", "Title");
                    ViewData["WarehouseId"] = new SelectList(_warehouses, "Id", "Title");
                    ViewData["ProductId"] = new SelectList(await _context.Products.Select(p => new { p.Id, p.Title }).AsNoTracking().ToListAsync(), "Id", "Title");
                    return View(batch);
                }
                try
                {
                    if (batch.BranchId == 0)
                        batch.BranchId = null;
                    else
                        batch.WarehouseId = null;

                    _uploadService.DeleteFile(batch.QrImageUrl);
                    string qrText = String.Format("batchId:{0}\nproductId:{1}", batch.Id, batch.ProductId);
                    batch.QrImageUrl = await _qrService.CreateQRCodeAsync(qrText);

                    _context.Update(batch);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BatchExists(batch.Id))
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
            var branches = await _context.Branches.Select(b => new { b.Id, b.Title }).AsNoTracking().ToListAsync();
            var warehouses = await _context.Warehouses.Select(w => new { w.Id, w.Title }).AsNoTracking().ToListAsync();
            branches.Insert(0, new { Id = 0, Title = "None" });
            warehouses.Insert(0, new { Id = 0, Title = "None" });

            ViewData["BranchId"] = new SelectList(branches, "Id", "Title");
            ViewData["WarehouseId"] = new SelectList(warehouses, "Id", "Title");
            ViewData["ProductId"] = new SelectList(await _context.Products.Select(p => new { p.Id, p.Title }).AsNoTracking().ToListAsync(), "Id", "Title");
            return View(batch);
        }

        // GET: Batches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var batch = await _context.Batches
                .Include(b => b.Branch)
                .Include(b => b.Product)
                .Include(b => b.Warehouse)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (batch == null)
            {
                return NotFound();
            }

            return View(batch);
        }

        // POST: Batches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var batch = await _context.Batches.FindAsync(id);
            _context.Batches.Remove(batch);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BatchExists(int id)
        {
            return _context.Batches.Any(e => e.Id == id);
        }
    }
}
