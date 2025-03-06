using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using TechFixSolution.QuotationServices.Data;
using TechFixSolution.QuotationServices.Models;

namespace TechFixSolution.QuotationServices.Services
{
    public class QuotationService
    {
        private readonly QuotationContext _context;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public QuotationService(QuotationContext context, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient("InventoryClient"); // Use named HttpClient
        }

        // Create a new quotation (Supplier submits a quote)
        public async Task<Quotation> SubmitQuote(Quotation quotation)
        {
            // Check if the product exists in the inventory via API call to InventoryService
            var isProductAvailable = await CheckInventoryAvailability(quotation.ProductName);
            if (!isProductAvailable)
            {
                throw new InvalidOperationException("Product is out of stock.");
            }

            // Set default status to "Pending" when a supplier submits a quotation
            quotation.Status = "Pending";
            _context.Quotations.Add(quotation);
            _context.SaveChanges();
            return quotation;
        }

        // Get all quotations for Admin
        public IQueryable<Quotation> GetAllQuotes()
        {
            return _context.Quotations;
        }

        // Get a quotation by ID
        public Quotation GetQuotationById(int id)
        {
            return _context.Quotations.FirstOrDefault(q => q.Id == id);
        }

        // Approve or reject a quotation for Admin
        public string ApproveQuote(int id, string status)
        {
            var quote = _context.Quotations.FirstOrDefault(q => q.Id == id);
            if (quote == null) return "Quotation not found";

            quote.Status = status;
            _context.SaveChanges();
            return $"Quotation {status} successfully.";
        }

        // Update a quotation for Supplier
        public string UpdateQuote(int id, Quotation updatedQuote)
        {
            var quote = _context.Quotations.FirstOrDefault(q => q.Id == id);
            if (quote == null) return "Quotation not found";

            quote.ProductName = updatedQuote.ProductName;
            quote.Price = updatedQuote.Price;
            quote.Status = updatedQuote.Status;
            quote.SupplierName = updatedQuote.SupplierName;
            _context.SaveChanges();
            return "Quotation updated successfully.";
        }

        // Delete a quotation for Admin
        public bool DeleteQuotation(int id)
        {
            var quote = _context.Quotations.FirstOrDefault(q => q.Id == id);
            if (quote == null) return false;

            _context.Quotations.Remove(quote);
            _context.SaveChanges();
            return true;
        }

        // Check inventory availability through an API call to InventoryService
        private async Task<bool> CheckInventoryAvailability(string productName)
        {
            var response = await _httpClient.GetAsync($"api/inventory/check/{productName}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var inventoryResponse = JsonConvert.DeserializeObject<InventoryResponse>(content);

                return inventoryResponse?.IsAvailable ?? false;
            }

            return false;
        }
    }

    // Helper class for inventory response structure
    public class InventoryResponse
    {
        public bool IsAvailable { get; set; }
    }
}