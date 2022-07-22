using _03_FunctionsExercises.Data;
using _03_FunctionsExercises.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SendGrid.Helpers.Mail;

namespace _03_FunctionsExercises
{
    public class UpdateStatusToCompleteAndSendEmail
    {
        private readonly AzureSalesRequestDbContext _db;

        public UpdateStatusToCompleteAndSendEmail(AzureSalesRequestDbContext db)
        {
            _db = db;
        }
        [FunctionName("UpdateStatusToCompleteAndSendEmail")]
        public async Task Run([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer,
            [SendGrid(ApiKey = "CustomSendGridKey")] IAsyncCollector<SendGridMessage> messageCollector,
            ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            IEnumerable<SalesRequest> salesRequestFromDb = _db.SalesRequests.Where(u => u.Status == "Image Processed");
            foreach (var salesReq in salesRequestFromDb)
            {
                salesReq.Status = "Completed";
            }

            _db.UpdateRange(salesRequestFromDb);
            _db.SaveChanges();

            var message = new SendGridMessage();
            message.AddTo("johnalbh@gmail.com");
            message.AddContent("text/html", $"Processing Completed for {salesRequestFromDb.Count()} records");
            message.SetFrom(new EmailAddress("webmaster@cigarra.org"));
            message.SetSubject("Course Udemy");

            await messageCollector.AddAsync(message);

        }
    }
}
