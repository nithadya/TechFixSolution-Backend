namespace TechFixSolution.PaymentServices.Models
{
    public class PaymentModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }  // Link to the Order
        public decimal Amount { get; set; }
        public string Status { get; set; }  // e.g., Pending, Completed, Failed
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }  // e.g., Credit Card, PayPal, etc.
    }
}
