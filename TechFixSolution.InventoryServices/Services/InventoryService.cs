using TechFixSolution.InventoryServices.Data;
using TechFixSolution.InventoryServices.Models;
using System.Linq;

namespace TechFixSolution.InventoryServices.Services
{
    public class InventoryService
    {
        private readonly InventoryContext _context;

        public InventoryService(InventoryContext context)
        {
            _context = context;
        }

        // Create a new inventory item
        public InventoryModel AddInventoryItem(InventoryModel item)
        {
            _context.InventoryItems.Add(item);
            _context.SaveChanges();
            return item;
        }

        // Get all inventory items
        public IQueryable<InventoryModel> GetAllInventoryItems()
        {
            return _context.InventoryItems;
        }

        // Get a single inventory item by name
        public InventoryModel GetInventoryItemByName(string productName)
        {
            return _context.InventoryItems.FirstOrDefault(item => item.ProductName.ToLower() == productName.ToLower());
        }

        // Update an existing inventory item
        public InventoryModel UpdateInventoryItem(int id, InventoryModel updatedItem)
        {
            var item = _context.InventoryItems.FirstOrDefault(i => i.Id == id);
            if (item == null) return null;

            item.ProductName = updatedItem.ProductName ?? item.ProductName;
            item.StockQuantity = updatedItem.StockQuantity;
            item.Price = updatedItem.Price;

            _context.SaveChanges();
            return item;
        }

        // Delete an inventory item
        public bool DeleteInventoryItem(int id)
        {
            var item = _context.InventoryItems.FirstOrDefault(i => i.Id == id);
            if (item == null) return false;

            _context.InventoryItems.Remove(item);
            _context.SaveChanges();
            return true;
        }

        // Check product availability
        public bool CheckProductAvailability(string productName)
        {
            var item = _context.InventoryItems.FirstOrDefault(i => i.ProductName.ToLower() == productName.ToLower());
            return item != null && item.StockQuantity > 0;
        }
    }
}