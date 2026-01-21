using Invoicing.Domain.Models.ValueObjects;
using static Invoicing.Domain.Models.Entities.Invoice;

namespace Invoicing.Domain.Operations;

internal sealed class ValidateInvoiceOperation : InvoiceOperation
{
    protected override IInvoice OnUnvalidated(UnvalidatedInvoice invoice)
    {
        var errors = new List<string>();
        var cmd = invoice.Command;

        if (cmd.BookingId <= 0)
            errors.Add($"Invalid booking ID: {cmd.BookingId}");

        if (!CustomerName.TryParse(cmd.CustomerName, out var customerName))
            errors.Add($"Invalid customer name: {cmd.CustomerName}");

        if (!CustomerEmail.TryParse(cmd.CustomerEmail, out var customerEmail))
            errors.Add($"Invalid customer email: {cmd.CustomerEmail}");

        if (string.IsNullOrWhiteSpace(cmd.RoomType))
            errors.Add($"Invalid room type: {cmd.RoomType}");

        if (string.IsNullOrWhiteSpace(cmd.RoomNumber))
            errors.Add($"Invalid room number: {cmd.RoomNumber}");

        if (string.IsNullOrWhiteSpace(cmd.PaymentTransactionId))
            errors.Add($"Invalid payment transaction ID: {cmd.PaymentTransactionId}");

        if (!MoneyValue.TryParse(cmd.TotalAmount, out var netValue))
            errors.Add($"Invalid total amount: {cmd.TotalAmount}");

        if (errors.Any())
            return new InvalidInvoice(errors);

        return new ValidatedInvoice(
            cmd.BookingId, 
            customerName!, 
            customerEmail!,
            cmd.RoomType,
            cmd.RoomNumber,
            cmd.CheckInDate,
            cmd.CheckOutDate,
            netValue!,
            cmd.PaymentTransactionId);
    }
}
