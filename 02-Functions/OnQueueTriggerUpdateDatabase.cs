using System;
using _02_Functions.Data;
using _02_Functions.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace _02_Functions
{
    public class OnQueueTriggerUpdateDatabase
    {
        private readonly AzureCourseDBContext _db;

        public OnQueueTriggerUpdateDatabase(AzureCourseDBContext db)
        {
            _db = db;
        }
        [FunctionName("OnQueueTriggerUpdateDatabase")]
        public void Run([QueueTrigger("salesrequestinbound", Connection = "AzureWebJobsStorage")]SalesRequest myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            myQueueItem.Status = "Submitted";
            _db.SalesRequests.Add(myQueueItem);
            _db.SaveChanges();
        }
    }
}
