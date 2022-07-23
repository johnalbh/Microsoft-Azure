using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureSpookyLogic.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AzureSpookyLogic.Controllers;
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    static readonly HttpClient client = new HttpClient();
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(SpookyRequest spookyRequest, IFormFile file)
    {
        spookyRequest.Id = Guid.NewGuid().ToString();
        var jsonContent = JsonConvert.SerializeObject(spookyRequest);
        using (var content = new StringContent(jsonContent, Encoding.UTF8, "application/json"))
        {
            HttpResponseMessage httpResponse = await client.PostAsync("https://prod-46.eastus2.logic.azure.com:443/workflows/c3e982ddda3547e4a62c7ca9f1f724ec/triggers/manual/paths/invoke?api-version=2016-10-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=JSUk-oCN-2KUm8bOabYjR12fcwr47CAu-sI5baqyL70", content);
        }

        return RedirectToAction(nameof(Index));
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