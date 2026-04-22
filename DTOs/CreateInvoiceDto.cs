using System.ComponentModel.DataAnnotations;

namespace InventoryBilling.API.DTOs
{
    public class CreateInvoiceDto
    {
        [Required]
        public List<InvoiceItemDto> Items { get; set; }
    }
    public class InvoiceItemDto
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
