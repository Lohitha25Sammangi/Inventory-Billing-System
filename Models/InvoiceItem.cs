using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryBilling.API.Models
{
    public class InvoiceItem
    {
        public int Id { get; set; }
        [Required]
        public int InvoiceId { get; set; }
        [ForeignKey("InvoiceId")]
        public Invoice Invoice { get; set; }
        [Required]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        [Required]
        [Range(1,10000)]
        public int Quantity { get; set; }
        [Required]
        [Column(TypeName ="decimal(18,2)")]
        public decimal UnitPrice { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxAmount { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

    }
}