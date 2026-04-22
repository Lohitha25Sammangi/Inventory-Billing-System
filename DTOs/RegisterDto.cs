using System.ComponentModel.DataAnnotations;

namespace InventoryBilling.API.DTOs
{
    public class RegisterDto
    {
        [Required]
    [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }  // Admin / StoreManager / Cashier
    }
}

