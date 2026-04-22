using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryBilling.API.Models
{
    public class StockRecord
    {
        public int Id { get; set; }
        [Required]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        [Required]
        [StringLength(20)]
        public string ChangeType { get; set; }
        [Required]
        public int QuantityChanged { get; set; }
        [StringLength(200)]
        public string? Remarks { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
