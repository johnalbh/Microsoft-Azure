﻿using Azure_Functions.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Newtonsoft.Json;


namespace Azure_Functions.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient client = new HttpClient();
        private readonly BlobServiceClient _blobServiceClient; 

        public HomeController(ILogger<HomeController> logger, BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        //
        [HttpPost]
        public async Task<IActionResult> Index(SalesRequest salesRequest, IFormFile file)
        {
            salesRequest.Id = Guid.NewGuid().ToString();
            using (var content = new StringContent(JsonConvert.SerializeObject(salesRequest), System.Text.Encoding.UTF8,
                       "application/json"))
            {
                HttpResponseMessage response =
                    await client.PostAsync("http://localhost:7071/api/CourseOnSalesUploadWriteToQueue", content);
                string returnValue = response.Content.ReadAsStringAsync().Result;
            }

            if (file != null)
            {
                var fileName = salesRequest.Id + Path.GetExtension(file.Name);
                BlobContainerClient blobContainerClient =
                    _blobServiceClient.GetBlobContainerClient("function-sales-request");
                var blobClient = blobContainerClient.GetBlobClient(fileName);

                var httpHeader = new BlobHttpHeaders
                {
                    ContentType = file.ContentType
                };

                await blobClient.UploadAsync(file.OpenReadStream(), httpHeader);
                return View();
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
}