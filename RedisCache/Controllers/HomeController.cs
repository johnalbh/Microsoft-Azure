using Microsoft.AspNetCore.Mvc;
using RedisCache.Models;
using System.Diagnostics;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RedisCache.Data;

namespace RedisCache.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IDistributedCache _cache;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, IDistributedCache cache)
        {
            _logger = logger;
            _db = db;
            _cache = cache;
        }

        public IActionResult Index()
        {
            List<Category> categoryList = new();
            var cachedCategory = _cache.GetString("categoryList");
            if (!string.IsNullOrEmpty(cachedCategory))
            {
                //cache
                categoryList = JsonConvert.DeserializeObject<List<Category>>(cachedCategory);
            }
            else
            {
                categoryList = _db.Categories.ToList();
                DistributedCacheEntryOptions options = new();
                options.SetAbsoluteExpiration(new TimeSpan(0, 0, 30));
                _cache.SetString("categoryList", JsonConvert.SerializeObject(categoryList), options);
            }

            return View(categoryList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}