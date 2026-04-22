using System.ComponentModel.DataAnnotations;

namespace InventoryBilling.API.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(250)]
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }=DateTime.UtcNow;
        public ICollection<Product>? Products { get; set; }
    }
}
