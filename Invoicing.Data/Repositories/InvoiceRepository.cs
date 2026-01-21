using Invoicing.Data.Models;
using Invoicing.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using static Invoicing.Domain.Models.Entities.Invoice;

namespace Invoicing.Data.Repositories;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly InvoicingDbContext _context;

    public InvoiceRepository(InvoicingDbContext context)
    {
        _context = context;
    }

    public async Task<int> SaveInvoiceAsync(IssuedInvoice invoice)
    {
        var entity = new InvoiceEntity
        {
            BookingId = invoice.BookingId,
            InvoiceNumber = invoice.InvoiceNumber.Value,
            CustomerName = invoice.CustomerName.Value,
            CustomerEmail = invoice.CustomerEmail.Value,
            RoomType = invoice.RoomType,
            RoomNumber = invoice.RoomNumber,
            CheckInDate = invoice.CheckInDate,
            CheckOutDate = invoice.CheckOutDate,
            NetValue = invoice.NetValue.Value,
            VatValue = invoice.VatValue.Value,
            TotalValue = invoice.TotalValue.Value,
            PaymentTransactionId = invoice.PaymentTransactionId,
            IssuedDate = invoice.IssuedDate
        };

        _context.Invoices.Add(entity);
        await _context.SaveChangesAsync();

        return entity.InvoiceId;
    }

    public async Task<bool> InvoiceExistsForBookingAsync(int bookingId)
    {
        return await _context.Invoices.AnyAsync(i => i.BookingId == bookingId);
    }
}


