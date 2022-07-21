using System;
using _03_FunctionsExercises;
using _03_FunctionsExercises.Data;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

[assembly: WebJobsStartup(typeof(Startup))]
namespace _03_FunctionsExercises
{
    public class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            var connectionString = Environment.GetEnvironmentVariable("AzureSqlDatabase");
            builder.Services.AddDbContext<AzureSalesRequestDbContext>(opt => opt.UseSqlServer(connectionString));
            builder.Services.BuildServiceProvider();
        }
    }
}
