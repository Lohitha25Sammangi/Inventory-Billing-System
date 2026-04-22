using InventoryBilling.API.Data;
using InventoryBilling.API.DTOs;
using InventoryBilling.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InventoryBilling.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController:ControllerBase
    {
        private readonly AppDbContext _context;
        public InvoiceController(AppDbContext context) { 
            _context=context;   
        }
        [Authorize(Roles="Admin,Cashier")]
        [HttpPost]
        public async Task<IActionResult> CreateInvoice(CreateInvoiceDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(dto.Items==null || !dto.Items.Any())
            {
                return BadRequest("Invoice must contain at least one product");
            }
            using var transaction =await _context.Database.BeginTransactionAsync();
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                {
                    return BadRequest("UserId is null Token missing");
                }
                var invoice = new Invoice
                {
                    InvoiceNumber=$"INV-{DateTime.UtcNow.Ticks}",
                    CreatedAt = DateTime.UtcNow,
                    CreatedById = userId,
                    InvoiceItems = new List<InvoiceItem>()

                };
                decimal totalAmount = 0;
                foreach(var item in dto.Items)
                {
                    var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);
                    if (product == null)
                    {
                        return NotFound($"Product with ID {item.ProductId} is not found");

                    }
                    if (item.Quantity <= 0)
                    {
                        return BadRequest("Quantity must be greater than Zero");
                    }
                    if (product.stockQuantity < item.Quantity)
                    {
                        return BadRequest($"Insufficient stock for {product.Name}, Available: {product.stockQuantity}");

                    }
                    product.stockQuantity -= item.Quantity;
                    var invoiceItem = new InvoiceItem
                    {
                        ProductId = product.Id,
                        Quantity = item.Quantity,
                        UnitPrice = product.price,
                        TotalPrice = product.price * item.Quantity,
                    };
                    totalAmount += invoiceItem.TotalPrice;
                    invoice.InvoiceItems.Add(invoiceItem);
                }
                invoice.TotalAmount = totalAmount;
                decimal tax = totalAmount * 0.10m;
                invoice.TaxAmount = tax;
                invoice.GrandTotal = totalAmount + tax;
                _context.Invoices.Add(invoice);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok(new
                {
                    Message = "Invoice created successfully",
                    InvoiceId = invoice.Id,
                    totalAmount = invoice.TotalAmount
                });
            }
            catch(Exception e)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new
                {
                    Message = "Something went wrong",
                    Error = e.InnerException?.Message ?? e.Message
                });
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllInvoices()
        {
            var invoices=await _context.Invoices
                .Include(p=>p.InvoiceItems)
                  .ThenInclude(ii=>ii.Product)
                .OrderByDescending(i=>i.CreatedAt)
                .ToListAsync();
            return Ok(invoices);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInvoice(int id)
        {
            var invoice = await _context.Invoices
                .Include(i => i.InvoiceItems)
                .ThenInclude(ii => ii.Product)
                .FirstOrDefaultAsync(i => i.Id == id);
            if (invoice == null)
            {
                return NotFound("Invoice not found");
            }
            return Ok(invoice);

        }

    }
}
