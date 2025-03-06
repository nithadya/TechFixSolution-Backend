using Microsoft.AspNetCore.Mvc;
using TechFixSolution.OrderServices.Services;

namespace TechFixSolution.OrderServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        // Get all orders
        [HttpGet]
        public IActionResult GetAllOrders()
        {
            var orders = _orderService.GetAllOrders();
            return Ok(orders);
        }

        // Get a specific order by ID
        [HttpGet("{id}")]
        public IActionResult GetOrderById(int id)
        {
            var order = _orderService.GetOrderById(id);
            if (order == null) return NotFound("Order not found");

            return Ok(order);
        }

        // Create a new order (only approved quotations)
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            try
            {
                var newOrder = await _orderService.CreateOrder(request.QuotationId, request.CustomerName, request.Quantity);
                return CreatedAtAction(nameof(GetOrderById), new { id = newOrder.Id }, newOrder);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Update order status
        [HttpPut("status/{id}")]
        public IActionResult UpdateOrderStatus(int id, [FromBody] string status)
        {
            var result = _orderService.UpdateOrderStatus(id, status);
            if (result.Contains("not found"))
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        // Delete an order
        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(int id)
        {
            var result = _orderService.DeleteOrder(id);
            if (!result)
            {
                return NotFound("Order not found");
            }

            return Ok("Order deleted successfully.");
        }
    }

    // Helper class for creating an order
    public class CreateOrderRequest
    {
        public int QuotationId { get; set; }
        public string CustomerName { get; set; }
        public int Quantity { get; set; }
    }
}