namespace TechFixSolution.InventoryServices.Models
{
    public class InventoryModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int StockQuantity { get; set; }
        public decimal Price { get; set; }
    }
}

