using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace TechFixSolution.API_Gateways.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GatewayController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public GatewayController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        #region Authentication Service Routes
        [HttpPost("auth/login")]
        public async Task<IActionResult> Login([FromBody] object loginRequest)
        {
            var baseUrl = _configuration["Services:AuthService"];
            var response = await _httpClient.PostAsJsonAsync($"{baseUrl}/api/auth/login", loginRequest);
            return Content(await response.Content.ReadAsStringAsync(), "application/json");
        }

        [HttpPost("auth/register")]
        public async Task<IActionResult> Register([FromBody] object registerRequest)
        {
            var baseUrl = _configuration["Services:AuthService"];
            var response = await _httpClient.PostAsJsonAsync($"{baseUrl}/api/auth/register", registerRequest);
            return Content(await response.Content.ReadAsStringAsync(), "application/json");
        }

        [HttpGet("auth/user/{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var baseUrl = _configuration["Services:AuthService"];
            var response = await _httpClient.GetAsync($"{baseUrl}/api/auth/user/{id}");
            return Content(await response.Content.ReadAsStringAsync(), "application/json");
        }
        #endregion

        #region Quotation Service Routes
        [HttpGet("quotations")]
        public async Task<IActionResult> GetQuotations()
        {
            var baseUrl = _configuration["Services:QuotationService"];
            var response = await _httpClient.GetAsync($"{baseUrl}/api/quotation");
            return Content(await response.Content.ReadAsStringAsync(), "application/json");
        }

        [HttpPost("quotations")]
        public async Task<IActionResult> SubmitQuotation([FromBody] object quotation)
        {
            var baseUrl = _configuration["Services:QuotationService"];
            var response = await _httpClient.PostAsJsonAsync($"{baseUrl}/api/quotation", quotation);
            return Content(await response.Content.ReadAsStringAsync(), "application/json");
        }
        #endregion

        #region Inventory Service Routes
        [HttpGet("inventory")]
        public async Task<IActionResult> GetInventory()
        {
            var baseUrl = _configuration["Services:InventoryService"];
            var response = await _httpClient.GetAsync($"{baseUrl}/api/inventory");
            return Content(await response.Content.ReadAsStringAsync(), "application/json");
        }
        #endregion

        #region Order Service Routes
        [HttpPost("orders")]
        public async Task<IActionResult> CreateOrder([FromBody] object order)
        {
            var baseUrl = _configuration["Services:OrderService"];
            var response = await _httpClient.PostAsJsonAsync($"{baseUrl}/api/order", order);
            return Content(await response.Content.ReadAsStringAsync(), "application/json");
        }
        #endregion

        #region Payment Service Routes
        [HttpPost("payments")]
        public async Task<IActionResult> CreatePayment([FromBody] object payment)
        {
            var baseUrl = _configuration["Services:PaymentService"];
            var response = await _httpClient.PostAsJsonAsync($"{baseUrl}/api/payment", payment);
            return Content(await response.Content.ReadAsStringAsync(), "application/json");
        }
        #endregion
    }
}
