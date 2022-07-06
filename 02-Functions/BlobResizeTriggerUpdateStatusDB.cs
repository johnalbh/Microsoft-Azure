using System;
using System.IO;
using System.Linq;
using _02_Functions.Data;
using _02_Functions.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace _02_Functions
{
    public class BlobResizeTriggerUpdateStatusDB
    {
        public readonly AzureCourseDBContext _db;

        public BlobResizeTriggerUpdateStatusDB(AzureCourseDBContext db)
        {
            _db = db;
        }
        [FunctionName("BlobResizeTriggerUpdateStatusDB")]
        public void Run([BlobTrigger("function-sales-reques-sm/{name}", Connection = "AzureWebJobsStorage")]Stream myBlob, string name, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

            var fileName = Path.GetFileNameWithoutExtension(name);
            SalesRequest salesRequestFromDb = _db.SalesRequests.FirstOrDefault(u => u.Id == fileName);

            if (salesRequestFromDb != null)
            {
                salesRequestFromDb.Status = "Image Processed";
                _db.SalesRequests.Update(salesRequestFromDb);
                _db.SaveChanges();
            }
        }
    }
}
