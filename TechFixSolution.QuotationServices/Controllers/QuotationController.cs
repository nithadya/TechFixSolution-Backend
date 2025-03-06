using Microsoft.AspNetCore.Mvc;
using TechFixSolution.QuotationServices.Models;
using TechFixSolution.QuotationServices.Services;

namespace TechFixSolution.QuotationServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuotationController : ControllerBase
    {
        private readonly QuotationService _quotationService;

        public QuotationController(QuotationService quotationService)
        {
            _quotationService = quotationService;
        }

        [HttpGet]
        public IActionResult GetAllQuotes()
        {
            var quotes = _quotationService.GetAllQuotes();
            return Ok(quotes);
        }

        [HttpGet("{id}")]
        public IActionResult GetQuotationById(int id)
        {
            var quote = _quotationService.GetQuotationById(id);
            return quote == null ? NotFound("Quotation not found") : Ok(quote);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitQuote([FromBody] Quotation quotation)
        {
            try
            {
                var newQuote = await _quotationService.SubmitQuote(quotation);
                return CreatedAtAction(nameof(GetQuotationById), new { id = newQuote.Id }, newQuote);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("approve/{id}")]
        public IActionResult ApproveQuote(int id, [FromBody] string status)
        {
            var result = _quotationService.ApproveQuote(id, status);
            return result.Contains("not found") ? NotFound(result) : Ok(result);
        }

        [HttpPut("update/{id}")]
        public IActionResult UpdateQuote(int id, [FromBody] Quotation updatedQuote)
        {
            var result = _quotationService.UpdateQuote(id, updatedQuote);
            return result.Contains("not found") ? NotFound(result) : Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteQuotation(int id)
        {
            var result = _quotationService.DeleteQuotation(id);
            return result ? Ok("Quotation deleted successfully.") : NotFound("Quotation not found");
        }
    }
}