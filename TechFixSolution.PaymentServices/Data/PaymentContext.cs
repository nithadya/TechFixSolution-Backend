using Microsoft.EntityFrameworkCore;
using TechFixSolution.PaymentServices.Models;


namespace TechFixSolution.PaymentServices.Data
{
    public class PaymentContext : DbContext
    {
        public PaymentContext(DbContextOptions<PaymentContext> options) : base(options) { }

        public DbSet<PaymentModel> Payments { get; set; }
                protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the Price column to have a specific precision and scale
            modelBuilder.Entity<PaymentModel>()
                .Property(q => q.Amount)
                .HasColumnType("decimal(18,2)");  // Precision 18 and scale 2 (e.g., 9999999999999999.99)
        }
    }
}
