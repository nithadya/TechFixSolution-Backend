using Microsoft.EntityFrameworkCore;
using TechFixSolution.InventoryServices.Models;

namespace TechFixSolution.InventoryServices.Data
{
    public class InventoryContext : DbContext
    {
        public InventoryContext(DbContextOptions<InventoryContext> options) : base(options) { }

        public DbSet<InventoryModel> InventoryItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the Price column to have a specific precision and scale
            modelBuilder.Entity<InventoryModel>()
                .Property(q => q.Price)
                .HasColumnType("decimal(16,2)");  // Precision 18 and scale 2 (e.g., 9999999999999999.99)
        }
    }
}