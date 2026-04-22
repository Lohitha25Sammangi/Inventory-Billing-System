using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryBilling.API.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        [Required]
        [Range(0.01,999999)]
        [Column(TypeName ="decimal(18,2)")]
        public decimal price { get; set; }
        [Required]
        [Range(0,100)]
        public decimal TaxPercentage { get; set; }
        [Required]
        [Range(0,int.MaxValue)]
        public int stockQuantity { get; set; }
        [StringLength(50)]
        public string? BarCode { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<InvoiceItem>? InvoiceItems { get; set; }
    }
}