using InventoryBilling.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using InventoryBilling.API.DTOs;
using Microsoft.AspNetCore.Authorization;
using InventoryBilling.API.Models;

namespace InventoryBilling.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController:ControllerBase
    {
        private readonly AppDbContext _context;
        public ProductsController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet("GetAll")]
        [Authorize(Roles = "Admin,StoreManager,Cashier")]
        public async Task<IActionResult> GetAll()
        {
            var products = await _context.Products
        .Include(p => p.Category)
        .Select(p => new ProductResponseDto
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.price,
            StockQuantity = p.stockQuantity,
            CategoryName = p.Category.Name
        })
        .ToListAsync();
            return Ok(products);
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,StoreManager,Cashier")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _context.Products
        .Include(p => p.Category)
        .Where(p => p.Id == id)
        .Select(p => new ProductDetailDto
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.price,
            StockQuantity = p.stockQuantity,
            CategoryId = p.CategoryId,
            CategoryName = p.Category.Name,
            IsActive = p.IsActive
        })
        .FirstOrDefaultAsync();
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
        [HttpPost("Create")]
        [Authorize(Roles ="Admin,StoreManager")]
        public async Task<IActionResult> CreateProduct(createProductDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var product = new Product
            {
                Name = model.Name,
                CategoryId = model.CategoryId,
                price = model.price,
                TaxPercentage = model.TaxPercentage,
                stockQuantity = model.stockQuantity,
                BarCode = model.BarCode,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            var Record = new StockRecord
            {
                ProductId = product.Id,
                ChangeType = "Adding",
                QuantityChanged = model.stockQuantity,
                CreatedAt = DateTime.UtcNow
            };
            await _context.StockRecords.AddAsync(Record);
            await _context.SaveChangesAsync();
            return Ok("Product craeted successfully");
        }
        [HttpPut("Update/{id}")]
        [Authorize(Roles ="Admin,StoreManager")]
        public async Task<IActionResult> Update(int id,UpdateProductDto model)
        {
            if (!ModelState.IsValid)
            {
                return Ok(ModelState);           
            }
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            product.Name = model.Name;
            product.CategoryId = model.CategoryId;
            product.price = model.price;
            product.TaxPercentage = model.TaxPercentage;
            product.stockQuantity = model.stockQuantity;
            product.IsActive = model.IsActive;

            var changedQuantity = model.stockQuantity - product.stockQuantity;
            if (changedQuantity != 0)
            {
                var record = new StockRecord
                {
                    ProductId = product.Id,
                    ChangeType = "Adjusted",
                    QuantityChanged = changedQuantity,
                    CreatedAt = DateTime.UtcNow
                };
                await _context.StockRecords.AddAsync(record);
            }
            await _context.SaveChangesAsync();
     
            return Ok("record updated succesfully");
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            product.IsActive = false;
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return Ok("Product Deleted successfully");
                
        }
    }
}
