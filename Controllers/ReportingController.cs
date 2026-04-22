using InventoryBilling.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class ReportingController : ControllerBase
{
    private readonly AppDbContext _context;

    public ReportingController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSalesSummary()
    {
        var today = DateTime.UtcNow.Date;

        var totalSalesToday = await _context.Invoices
            .Where(i => i.CreatedAt.Date == today)
            .SumAsync(i => (decimal?)i.GrandTotal) ?? 0;

        var totalSalesMonth = await _context.Invoices
            .Where(i => i.CreatedAt.Month == today.Month &&
                        i.CreatedAt.Year == today.Year)
            .SumAsync(i => (decimal?)i.GrandTotal) ?? 0;

        var totalTax = await _context.Invoices
            .SumAsync(i => (decimal?)i.TaxAmount) ?? 0;

        var totalInvoices = await _context.Invoices.CountAsync();

        var paidInvoices = await _context.Invoices.CountAsync(i => i.IsPaid);

        var unpaidInvoices = await _context.Invoices.CountAsync(i => !i.IsPaid);

        var lowStockProducts = await _context.Products
            .Where(p => p.stockQuantity < 5)
            .Select(p => new
            {
                p.Id,
                p.Name,
                p.stockQuantity
            })
            .ToListAsync();

        return Ok(new
        {
            TotalSalesToday = totalSalesToday,
            TotalSalesThisMonth = totalSalesMonth,
            TotalTaxCollected = totalTax,
            TotalInvoices = totalInvoices,
            PaidInvoices = paidInvoices,
            UnpaidInvoices = unpaidInvoices,
            LowStockProducts = lowStockProducts
        });
    }
}