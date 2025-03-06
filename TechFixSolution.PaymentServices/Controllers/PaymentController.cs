using Microsoft.AspNetCore.Mvc;
using TechFixSolution.PaymentServices.Services;

namespace TechFixSolution.PaymentServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentService _paymentService;

        public PaymentController(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // Get all payments
        [HttpGet]
        public IActionResult GetAllPayments()
        {
            var payments = _paymentService.GetAllPayments();
            return Ok(payments);
        }

        // Get a specific payment by ID
        [HttpGet("{id}")]
        public IActionResult GetPaymentById(int id)
        {
            var payment = _paymentService.GetPaymentById(id);
            if (payment == null) return NotFound("Payment not found");

            return Ok(payment);
        }

        // Process a new payment
        [HttpPost]
        public async Task<IActionResult> ProcessPayment([FromBody] CreatePaymentRequest request)
        {
            try
            {
                var payment = await _paymentService.ProcessPayment(request.OrderId, request.Amount, request.PaymentMethod);
                return CreatedAtAction(nameof(GetPaymentById), new { id = payment.Id }, payment);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Update payment status
        [HttpPut("{id}")]
        public IActionResult UpdatePaymentStatus(int id, [FromBody] string status)
        {
            var result = _paymentService.UpdatePaymentStatus(id, status);
            if (result.Contains("not found"))
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        // Delete a payment
        [HttpDelete("{id}")]
        public IActionResult DeletePayment(int id)
        {
            var result = _paymentService.DeletePayment(id);
            if (!result)
            {
                return NotFound("Payment not found");
            }

            return Ok("Payment deleted successfully.");
        }
    }

    // Helper class for creating a payment
    public class CreatePaymentRequest
    {
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
    }
}