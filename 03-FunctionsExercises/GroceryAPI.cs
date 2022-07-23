using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using _03_FunctionsExercises.Data;
using _03_FunctionsExercises.Models;

namespace _03_FunctionsExercises
{
    public  class GroceryAPI
    {
        private readonly AzureSalesRequestDbContext _db;

        public GroceryAPI(AzureSalesRequestDbContext db)
        {
            _db = db;
        }

        [FunctionName("GroceryAPI")]
        public async Task<IActionResult> CreateGrocery(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "GroceryList")] HttpRequest req,
            ILogger log)
        {
            try
            {
                log.LogInformation("Creating Grocery List Item");
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                GroceryItem_Upsert data = JsonConvert.DeserializeObject<GroceryItem_Upsert>(requestBody);

                var groceryItem = new GroceryItem
                {
                    Name = data.Name
                };

                _db.GroceryItems.Add(groceryItem);
                _db.SaveChanges();

                return new OkObjectResult(groceryItem);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        [FunctionName("GetGrocery")]
        public async Task<IActionResult> GetGrocery(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "GroceryList")] HttpRequest req,
            ILogger log)
        {
            try
            {
                log.LogInformation("Getting all Grocery List Item");

                return new OkObjectResult(_db.GroceryItems.ToList());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        [FunctionName("GetGroceryById")]
        public async Task<IActionResult> GetGroceryById(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "GroceryList/{id}")] HttpRequest req,
            ILogger log, string id)
        {
            try
            {
                log.LogInformation("Getting all Grocery List Item by ID");
                var item = _db.GroceryItems.FirstOrDefault(u => u.Id == id);
                if (item == null)
                {
                    return new NotFoundResult();
                }

                return new OkObjectResult(item);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        [FunctionName("UpdateGrocery")]
        public async Task<IActionResult> UpdateGrocery(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "GroceryList/{id}")] HttpRequest req,
            ILogger log, string id)
        {
            try
            {
                log.LogInformation("Updating Grocery List Item");
                var item = _db.GroceryItems.FirstOrDefault(u => u.Id == id);
                if (item == null)
                {
                    return new NotFoundResult();
                }

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                GroceryItem_Upsert updatedData = JsonConvert.DeserializeObject<GroceryItem_Upsert>(requestBody);

                if (!string.IsNullOrEmpty(updatedData.Name))
                {
                    item.Name = updatedData.Name;
                };

                _db.GroceryItems.Update(item);
                _db.SaveChanges();

                return new OkObjectResult(item);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        [FunctionName("DeleteAPI")]
        public async Task<IActionResult> DeleteGrocery(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "GroceryList/{id}")] HttpRequest req,
            ILogger log, string id)
        {
            try
            {
                log.LogInformation("Delete Grocery List Item");
                var item = _db.GroceryItems.FirstOrDefault(u => u.Id == id);
                if (item == null)
                {
                    return new NotFoundResult();
                }
                _db.GroceryItems.Remove(item);
                _db.SaveChanges();

                return new OkResult();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
