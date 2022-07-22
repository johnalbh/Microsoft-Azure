using _03_FunctionsExercises.Data;
using _03_FunctionsExercises.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;

namespace _03_FunctionsExercises
{
    public class BlobResizeTriggerUpdateStatusDb
    {
        private readonly AzureSalesRequestDbContext _db;

        public BlobResizeTriggerUpdateStatusDb(AzureSalesRequestDbContext db)
        {
            _db = db;
        }

        [FunctionName("BlobResizeTriggerUpdateStatusDb")]
        public void Run([BlobTrigger("function-sales-request-sm/{name}", Connection = "")]Stream myBlob, string name, ILogger log)
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
