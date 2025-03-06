using Microsoft.EntityFrameworkCore;
using TechFixSolution.QuotationServices.Models;

namespace TechFixSolution.QuotationServices.Data
{
    public class QuotationContext : DbContext
    {
        public QuotationContext(DbContextOptions<QuotationContext> options) : base(options) { }

        public DbSet<Quotation> Quotations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the Price column to have a specific precision and scale
            modelBuilder.Entity<Quotation>()
                .Property(q => q.Price)
                .HasColumnType("decimal(18,2)");  // Precision 18 and scale 2 (e.g., 9999999999999999.99)
        }
    }
}