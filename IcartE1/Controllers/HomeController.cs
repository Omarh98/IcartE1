using IcartE1.Data;
using IcartE1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http;

namespace IcartE1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult ProductManagement()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult StockManagement()
        {
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Sales([FromQuery] int productId, [FromQuery] int branchId, [FromQuery] bool isCash)
        {
            var dataList = new List<DataPoint>();

            if (productId > 0 && branchId == 0)
            {
                var sales = await _context.Sales.Where(s => s.ProductId == productId)
                    .Select(s => new { s.Date, s.Quantity, s.Product.Price }).ToListAsync();
                dataList = sales.Select(s => new DataPoint((long)s.Date.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds, isCash ? s.Quantity * s.Price : (double)s.Quantity)).ToList();

            }
            else if (branchId > 0 && productId > 0)
            {
                var sales = await _context.Sales.Where(s => s.BranchId == branchId && s.ProductId == productId)
                    .Select(s => new { s.Date, s.Quantity, s.Product.Price }).ToListAsync();
                dataList = sales.Select(s => new DataPoint((long)s.Date.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds, isCash ? s.Quantity * s.Price : (double)s.Quantity)).ToList();

            }
            else if (branchId > 0 && productId == 0)
            {
                var sales = await _context.Sales.Where(s => s.BranchId == branchId)
                    .GroupBy(s => s.Date).Select(g => new { Date = g.Key, Quantity = isCash ? g.Sum(g => g.Quantity * g.Product.Price) : g.Sum(g => g.Quantity) }).ToListAsync();
                dataList = sales.Select(s => new DataPoint((long)s.Date.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds, s.Quantity)).ToList();

            }
            else
            {
                var sales = await _context.Sales.GroupBy(s => s.Date)
                    .Select(g => new { Date = g.Key, Quantity = isCash ? g.Sum(g => g.Quantity * g.Product.Price) : g.Sum(g => g.Quantity) }).ToListAsync();
                dataList = sales.Select(s => new DataPoint((long)s.Date.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds, s.Quantity)).ToList();
            }

            ViewBag.ProductId = new SelectList(await _context.Products.ToListAsync(), "Id", "Title");
            ViewBag.BranchId = new SelectList(await _context.Branches.ToListAsync(), "Id", "Title");
            ViewBag.DataPoints = JsonSerializer.Serialize(dataList);
            return View(new SalesFilterViewModel { productId = productId, BranchId = branchId ,IsCash=isCash});
        }

        public async Task<IActionResult> Forecasting([FromQuery] int category, [FromQuery] int days, [FromServices] IHttpClientFactory clientFactory)
        {
            var forecastingViewModel = new ForecastingFilterViewModel { Category = (ForecastingFilterViewModel.CategoryEnum)category, Days = days };

            if(forecastingViewModel.Days > 0)
            {
                var categoryStr = forecastingViewModel.Category.ToString();

                var request = new HttpRequestMessage(HttpMethod.Get, $"https://gradprojectapi-56c5b.ondigitalocean.app/predict?category={categoryStr}&n_days={days}");
                ViewBag.Error = null;
                var client = clientFactory.CreateClient();
                var response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode) 
                { 
                    ViewBag.Error = true;
                    return View(forecastingViewModel); 
                }
                var content = await response.Content.ReadAsStringAsync();

                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                var predictions =  JsonSerializer.Deserialize<List<Prediction>>(content,jsonOptions);

                var dataPoints = predictions.Select(p => new DataPoint((long)p.Date.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds, p.Sales)).ToList();
                ViewBag.DataPoints = JsonSerializer.Serialize(dataPoints);
                ViewBag.Error = false;
            }

            return View(forecastingViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
