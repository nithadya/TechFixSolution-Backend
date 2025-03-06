namespace TechFixSolution.QuotationServices.Models
{
    public class Quotation
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; } // Pending, Approved, Rejected
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public DateTime DateRequested { get; set; } 
    }
}
