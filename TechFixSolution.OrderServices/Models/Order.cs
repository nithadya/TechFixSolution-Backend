namespace TechFixSolution.OrderServices.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int QuotationId { get; set; } // Link to the QuotationService
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }  // e.g., Pending, Processed, Shipped
        public DateTime OrderDate { get; set; }
        public string CustomerName { get; set; }
    }
}
