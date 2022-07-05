using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _02_Functions.Models;
using Microsoft.EntityFrameworkCore;

namespace _02_Functions.Data
{
    public class AzureCourseDBContext : DbContext
    {
        public AzureCourseDBContext(DbContextOptions<AzureCourseDBContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<SalesRequest> SalesRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<SalesRequest>(entity =>
            {
                entity.HasKey(c => c.Id);
            });
        }
    }
}
