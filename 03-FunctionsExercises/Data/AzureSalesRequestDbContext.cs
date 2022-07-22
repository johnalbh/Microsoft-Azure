using _03_FunctionsExercises.Models;
using Microsoft.EntityFrameworkCore;

namespace _03_FunctionsExercises.Data
{
    public class AzureSalesRequestDbContext : DbContext
    {
        public AzureSalesRequestDbContext(DbContextOptions<AzureSalesRequestDbContext> dbContextOptions) : base (dbContextOptions)
        {
        }

        public DbSet<SalesRequest> SalesRequests { get; set; }

        public DbSet<GroceryItem> GroceryItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<SalesRequest>(entity =>
            {
                entity.HasKey(c => c.Id);
            });

            modelBuilder.Entity<GroceryItem>(entity =>
            {
                entity.HasKey(c => c.Id);
            });
        }
    }
}
