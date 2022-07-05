using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace _02_Functions
{
    public class OnQueueTriggerUpdateDatabase
    {
        [FunctionName("OnQueueTriggerUpdateDatabase")]
        public void Run([QueueTrigger("SalesRequestInBound", Connection = "AzureWebJobStorage")]string myQueueItem, ILogger log)
        {
            log.
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
