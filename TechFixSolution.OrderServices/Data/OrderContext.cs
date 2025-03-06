using Microsoft.EntityFrameworkCore;
using TechFixSolution.OrderServices.Models;

namespace TechFixSolution.OrderServices.Data
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options) { }

        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the Price column to have a specific precision and scale
            modelBuilder.Entity<Order>()
                .Property(q => q.Price)
                .HasColumnType("decimal(18,2)");  // Precision 18 and scale 2 (e.g., 9999999999999999.99)
        }
    }
}