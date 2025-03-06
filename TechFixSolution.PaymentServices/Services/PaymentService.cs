using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using TechFixSolution.PaymentServices.Data;
using TechFixSolution.PaymentServices.Models;

namespace TechFixSolution.PaymentServices.Services
{
    public class PaymentService
    {
        private readonly PaymentContext _context;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _orderClient;
        private readonly HttpClient _quotationClient;

        public PaymentService(PaymentContext context, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _configuration = configuration;
            _orderClient = httpClientFactory.CreateClient("OrderClient");
            _quotationClient = httpClientFactory.CreateClient("QuotationClient");
        }

        // Process a new payment
        public async Task<PaymentModel> ProcessPayment(int orderId, decimal amount, string paymentMethod)
        {
            // Fetch order via API call to OrderService
            var order = await GetOrderByIdAsync(orderId);
            if (order == null)
                throw new InvalidOperationException("Order not found.");

            // Fetch the corresponding quotation via API call to QuotationService
            var quotation = await GetQuotationByIdAsync(order.QuotationId);
            if (quotation == null || quotation.Status != "Approved")
                throw new InvalidOperationException("Invalid or unapproved quotation.");

            // Create the payment
            var payment = new PaymentModel
            {
                OrderId = order.Id,
                Amount = amount,
                Status = "Pending",  // Initially set status as Pending
                PaymentDate = DateTime.UtcNow,
                PaymentMethod = paymentMethod
            };

            _context.Payments.Add(payment);
            _context.SaveChanges();
            return payment;
        }

        // Get all payments
        public IQueryable<PaymentModel> GetAllPayments()
        {
            return _context.Payments;
        }

        // Get a specific payment by ID
        public PaymentModel GetPaymentById(int id)
        {
            return _context.Payments.FirstOrDefault(p => p.Id == id);
        }

        // Update payment status
        public string UpdatePaymentStatus(int id, string status)
        {
            var payment = _context.Payments.FirstOrDefault(p => p.Id == id);
            if (payment == null) return "Payment not found";

            payment.Status = status;
            _context.SaveChanges();
            return $"Payment status updated to {status}.";
        }

        // Delete a payment
        public bool DeletePayment(int id)
        {
            var payment = _context.Payments.FirstOrDefault(p => p.Id == id);
            if (payment == null) return false;

            _context.Payments.Remove(payment);
            _context.SaveChanges();
            return true;
        }

        // Fetch order by ID via API call to OrderService
        private async Task<dynamic> GetOrderByIdAsync(int orderId)
        {
            var response = await _orderClient.GetAsync($"api/order/{orderId}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject(content);
            }

            return null;
        }

        // Fetch quotation by ID via API call to QuotationService
        private async Task<dynamic> GetQuotationByIdAsync(int quotationId)
        {
            var response = await _quotationClient.GetAsync($"api/quotation/{quotationId}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject(content);
            }

            return null;
        }
    }
}  