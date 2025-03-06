using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using TechFixSolution.OrderServices.Data;
using TechFixSolution.OrderServices.Models;

namespace TechFixSolution.OrderServices.Services
{
    public class OrderService
    {
        private readonly OrderContext _context;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _quotationClient;
        private readonly HttpClient _inventoryClient;

        public OrderService(OrderContext context, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _configuration = configuration;
            _quotationClient = httpClientFactory.CreateClient("QuotationClient");
            _inventoryClient = httpClientFactory.CreateClient("InventoryClient");
        }

        // Create an order (only for approved quotations)
        public async Task<Order> CreateOrder(int quotationId, string customerName, int quantity)
        {
            // Fetch the approved quotation via REST API call to QuotationService
            var quotation = await GetQuotationByIdAsync(quotationId);
            if (quotation == null || quotation.Status != "Approved")
                throw new InvalidOperationException("Quotation not approved or not found.");

            // Check if the product is available in the inventory via API call to InventoryService
            var isProductAvailable = await CheckProductAvailabilityAsync(quotation.ProductName);
            if (!isProductAvailable)
                throw new InvalidOperationException("Product is out of stock.");

            // Create the order
            var order = new Order
            {
                QuotationId = quotation.Id,
                ProductName = quotation.ProductName,
                Price = quotation.Price,
                Quantity = quantity,
                Status = "Pending", // Set status to "Pending" initially
                OrderDate = DateTime.UtcNow,
                CustomerName = customerName
            };

            _context.Orders.Add(order);
            _context.SaveChanges();
            return order;
        }

        // Get all orders
        public IQueryable<Order> GetAllOrders()
        {
            return _context.Orders;
        }

        // Get a specific order by ID
        public Order GetOrderById(int id)
        {
            return _context.Orders.FirstOrDefault(o => o.Id == id);
        }

        // Update order status
        public string UpdateOrderStatus(int id, string status)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == id);
            if (order == null) return "Order not found";

            order.Status = status;
            _context.SaveChanges();
            return $"Order status updated to {status}.";
        }

        // Delete an order
        public bool DeleteOrder(int id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == id);
            if (order == null) return false;

            _context.Orders.Remove(order);
            _context.SaveChanges();
            return true;
        }

        // Fetch approved quotation by ID via API call to QuotationService
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

        // Check product availability through an API call to InventoryService
        private async Task<bool> CheckProductAvailabilityAsync(string productName)
        {
            var response = await _inventoryClient.GetAsync($"api/inventory/check/{productName}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var inventoryResponse = JsonConvert.DeserializeObject<dynamic>(content);
                return inventoryResponse?.IsAvailable ?? false;
            }

            return false;
        }
    }
}