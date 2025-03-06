using Microsoft.AspNetCore.Mvc;
using TechFixSolution.InventoryServices.Models;
using TechFixSolution.InventoryServices.Services;

namespace TechFixSolution.InventoryServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly InventoryService _inventoryService;

        public InventoryController(InventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        // Get all inventory items
        [HttpGet]
        public IActionResult GetAllInventoryItems()
        {
            var inventoryItems = _inventoryService.GetAllInventoryItems();
            return Ok(inventoryItems);
        }

        // Get a specific inventory item by name
        [HttpGet("{productName}")]
        public IActionResult GetInventoryItemByName(string productName)
        {
            var item = _inventoryService.GetInventoryItemByName(productName);
            if (item == null)
                return NotFound("Product not found");

            return Ok(item);
        }

        // Add a new inventory item
        [HttpPost]
        public IActionResult AddInventoryItem([FromBody] InventoryModel item)
        {
            var newItem = _inventoryService.AddInventoryItem(item);
            return CreatedAtAction(nameof(GetInventoryItemByName), new { productName = newItem.ProductName }, newItem);
        }

        // Update an existing inventory item
        [HttpPut("{id}")]
        public IActionResult UpdateInventoryItem(int id, [FromBody] InventoryModel updatedItem)
        {
            var item = _inventoryService.UpdateInventoryItem(id, updatedItem);
            if (item == null)
                return NotFound("Item not found");

            return Ok(item);
        }

        // Delete an inventory item
        [HttpDelete("{id}")]
        public IActionResult DeleteInventoryItem(int id)
        {
            var result = _inventoryService.DeleteInventoryItem(id);
            if (!result)
                return NotFound("Item not found");

            return Ok("Item deleted successfully");
        }

        // Check product availability
        [HttpGet("check/{productName}")]
        public IActionResult CheckProductAvailability(string productName)
        {
            var isAvailable = _inventoryService.CheckProductAvailability(productName);
            if (isAvailable)
                return Ok(new { message = "Product is available in inventory" });

            return BadRequest(new { message = "Product is out of stock" });
        }
    }
}