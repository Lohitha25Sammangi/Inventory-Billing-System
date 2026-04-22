using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryBilling.API.DTOs
{
    public class UpdateProductDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        [Range(0.01, 999999)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal price { get; set; }
        [Required]
        [Range(0, 100)]
        public decimal TaxPercentage { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int stockQuantity { get; set; }
        public bool IsActive { get; set; }
    }
}