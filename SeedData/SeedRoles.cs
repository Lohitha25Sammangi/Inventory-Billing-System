using InventoryBilling.API.Models;
using Microsoft.AspNetCore.Identity;

namespace InventoryBilling.API.SeedData
{
    public class SeedRoles
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            string[] roles = { "Admin", "StoreManager", "Cashier" };
            foreach (var role in roles) { 
                if(!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
            var adminEmail = "admin@example.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var user = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "System Admin"
                };
                var result=await userManager.CreateAsync(user,"Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
          
            }
        }
    }
}
