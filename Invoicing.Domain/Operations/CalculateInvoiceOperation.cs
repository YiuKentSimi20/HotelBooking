using Invoicing.Domain.Models.ValueObjects;
using static Invoicing.Domain.Models.Entities.Invoice;

namespace Invoicing.Domain.Operations;

internal sealed class CalculateInvoiceOperation : InvoiceOperation
{
    private const decimal VatRate = 0.19m; // 19% VAT

    protected override IInvoice OnValidated(ValidatedInvoice invoice)
    {
        // NetValue is the booking total (already received from BookingPaidIntegrationEvent)
        var netValue = invoice.NetValue;

        // Calculate VAT (19% of net value)
        var vatAmount = netValue.Value * VatRate;
        if (!MoneyValue.TryParse(vatAmount, out var vatValue))
            return new InvalidInvoice(new[] { $"Invalid VAT calculation: {vatAmount}" });

        // Calculate total value (net + VAT)
        var totalAmount = netValue.Value + vatValue!.Value;
        if (!MoneyValue.TryParse(totalAmount, out var totalValue))
            return new InvalidInvoice(new[] { $"Invalid total calculation: {totalAmount}" });

        return new CalculatedInvoice(
            invoice.BookingId,
            invoice.CustomerName,
            invoice.CustomerEmail,
            invoice.RoomType,
            invoice.RoomNumber,
            invoice.CheckInDate,
            invoice.CheckOutDate,
            netValue,
            vatValue!,
            totalValue!,
            invoice.PaymentTransactionId);
    }
}


