using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryBilling.API.Models
{
    public class Payment
    {
        public int Id { get; set; }
        [Required]
        public int InvoiceId { get; set; }
        [ForeignKey("InvoiceId")]
        public Invoice Invoice { get; set; }
        [Required]
        [StringLength(50)]
        public string PaymentMethod { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        public DateTime PaidAt { get; set; }=DateTime.UtcNow;
    }
}