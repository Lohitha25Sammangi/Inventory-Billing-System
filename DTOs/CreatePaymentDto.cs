using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryBilling.API.DTOs
{
    public class CreatePaymentDto
    {
        [Required]
        public int InvoiceId { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        [Required]
        [StringLength(50)]
        public string PaymentMethod { get; set; }
    }
}
