namespace InventoryBilling.API.DTOs
{
    internal class ProductResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string CategoryName { get; set; }
    }
}