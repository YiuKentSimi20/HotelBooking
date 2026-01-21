using static Invoicing.Domain.Models.Entities.Invoice;

namespace Invoicing.Domain.Repositories;

public interface IInvoiceRepository
{
    Task<int> SaveInvoiceAsync(IssuedInvoice invoice);
    Task<bool> InvoiceExistsForBookingAsync(int bookingId);
}
