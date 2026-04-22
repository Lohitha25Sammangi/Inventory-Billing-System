namespace InventoryBilling.API.DTOs
{
    internal class ProductDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public object Price { get; set; }
        public object StockQuantity { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool IsActive { get; set; }
    }
}