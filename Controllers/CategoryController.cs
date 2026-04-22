using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryBilling.API.Models;
using InventoryBilling.API.Data;

namespace InventoryBilling.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        // ========================================
        // GET ALL ACTIVE CATEGORIES
        // ========================================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _context.Categories
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .ToListAsync();

            return Ok(categories);
        }

        // ========================================
        // GET CATEGORY BY ID
        // ========================================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);

            if (category == null)
                return NotFound("Category not found.");

            return Ok(category);
        }

        // ========================================
        // CREATE CATEGORY (Admin Only)
        // ========================================
        [Authorize(Roles = "Admin")]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] Category model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var exists = await _context.Categories
                .AnyAsync(c => c.Name.ToLower() == model.Name.ToLower());

            if (exists)
                return BadRequest("Category already exists.");

            var category = new Category
            {
                Name = model.Name,
                Description = model.Description,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Category created successfully",
                category.Id,
                category.Name
            });
        }

        // ========================================
        // UPDATE CATEGORY (Admin Only)
        // ========================================
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Category model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = await _context.Categories.FindAsync(id);

            if (category == null || !category.IsActive)
                return NotFound("Category not found.");

            var duplicate = await _context.Categories
                .AnyAsync(c => c.Id != id &&
                               c.Name.ToLower() == model.Name.ToLower());

            if (duplicate)
                return BadRequest("Another category with same name exists.");

            category.Name = model.Name;
            category.Description = model.Description;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Category updated successfully"
            });
        }

        // ========================================
        // SOFT DELETE CATEGORY (Admin Only)
        // ========================================
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return NotFound("Category not found.");

            if (category.Products != null && category.Products.Any())
                return BadRequest("Cannot delete category with existing products.");

            // Soft delete
            category.IsActive = false;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Category deactivated successfully"
            });
        }
    }
}