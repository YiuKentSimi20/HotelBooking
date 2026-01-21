using Invoicing.Domain.Models.ValueObjects;
using static Invoicing.Domain.Models.Entities.Invoice;

namespace Invoicing.Domain.Operations;

internal sealed class IssueInvoiceOperation : InvoiceOperation
{
    protected override IInvoice OnCalculated(CalculatedInvoice invoice)
    {
        // Generate invoice number
        var invoiceNumber = InvoiceNumber.Generate(invoice.BookingId);

        // Issue date is now
        var issuedDate = DateTime.UtcNow;

        return new IssuedInvoice(
            invoice.BookingId,
            invoiceNumber,
            invoice.CustomerName,
            invoice.CustomerEmail,
            invoice.RoomType,
            invoice.RoomNumber,
            invoice.CheckInDate,
            invoice.CheckOutDate,
            invoice.NetValue,
            invoice.VatValue,
            invoice.TotalValue,
            invoice.PaymentTransactionId,
            issuedDate);
    }
}


