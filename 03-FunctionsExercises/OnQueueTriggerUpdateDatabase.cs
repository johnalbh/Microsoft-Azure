using _03_FunctionsExercises.Data;
using _03_FunctionsExercises.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace _03_FunctionsExercises
{
    public class OnQueueTriggerUpdateDatabase
    {
        private readonly AzureSalesRequestDbContext _db;

        public OnQueueTriggerUpdateDatabase(AzureSalesRequestDbContext db)
        {
            _db = db;
        }
        [FunctionName("OnQueueTriggerUpdateDatabase")]
        public void Run([QueueTrigger("NewSalesRequestInBound", Connection = "AzureWebJobsStorage")]SalesRequest myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            myQueueItem.Status = "Submitted";
            _db.SalesRequests.Add(myQueueItem);
            _db.SaveChanges();
        }
    }
}
