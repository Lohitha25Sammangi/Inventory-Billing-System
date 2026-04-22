using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryBilling.API.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string InvoiceNumber { get; set; }
        [StringLength(150)]
        public string? CustomerName { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxAmount { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal GrandTotal { get; set; }
        [Required]
        public string CreatedById { get; set; }
        [ForeignKey("CreatedById")]
        public ApplicationUser User { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<InvoiceItem>? InvoiceItems { get; set; }
        public Payment? Payment { get; set; }
        public bool IsPaid { get; set; } = false;
        public DateTime? PaidAt { get; set; }

    }
}