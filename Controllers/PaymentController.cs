using InventoryBilling.API.DTOs;
using InventoryBilling.API.Models;
using InventoryBilling.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PaymentController : ControllerBase
{
    private readonly AppDbContext _context;

    public PaymentController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> MakePayment(CreatePaymentDto dto)
    {
        var invoice = await _context.Invoices
            .FirstOrDefaultAsync(i => i.Id == dto.InvoiceId);

        if (invoice == null)
            return NotFound("Invoice not found");

        if (invoice.IsPaid)
            return BadRequest("Invoice already paid");

        if (dto.Amount != invoice.GrandTotal)
            return BadRequest("Payment amount must equal invoice total");

        var payment = new Payment
        {
            InvoiceId = invoice.Id,
            Amount = dto.Amount,
            PaymentMethod = dto.PaymentMethod
        };

        invoice.IsPaid = true;
        invoice.PaidAt = DateTime.UtcNow;

        _context.Payments.Add(payment);

        await _context.SaveChangesAsync();

        return Ok(new
        {
            Message = "Payment successful",
            InvoiceId = invoice.Id,
            PaidAmount = payment.Amount
        });
    }
}