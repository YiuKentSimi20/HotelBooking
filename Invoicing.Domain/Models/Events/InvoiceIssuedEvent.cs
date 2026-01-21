using static Invoicing.Domain.Models.Entities.Invoice;

namespace Invoicing.Domain.Models.Events;

public static class InvoiceIssuedEvent
{
    public interface IInvoiceIssuedEvent { }

    public record InvoiceSucceeded : IInvoiceIssuedEvent
    {
        internal InvoiceSucceeded(
            int invoiceId,
            int bookingId,
            string invoiceNumber,
            string customerName,
            string customerEmail,
            string roomType,
            string roomNumber,
            DateTime checkInDate,
            DateTime checkOutDate,
            decimal netValue,
            decimal vatValue,
            decimal totalValue,
            string paymentTransactionId,
            DateTime issuedDate)
        {
            InvoiceId = invoiceId;
            BookingId = bookingId;
            InvoiceNumber = invoiceNumber;
            CustomerName = customerName;
            CustomerEmail = customerEmail;
            RoomType = roomType;
            RoomNumber = roomNumber;
            CheckInDate = checkInDate;
            CheckOutDate = checkOutDate;
            NetValue = netValue;
            VatValue = vatValue;
            TotalValue = totalValue;
            PaymentTransactionId = paymentTransactionId;
            IssuedDate = issuedDate;
        }

        public int InvoiceId { get; }
        public int BookingId { get; }
        public string InvoiceNumber { get; }
        public string CustomerName { get; }
        public string CustomerEmail { get; }
        public string RoomType { get; }
        public string RoomNumber { get; }
        public DateTime CheckInDate { get; }
        public DateTime CheckOutDate { get; }
        public decimal NetValue { get; }
        public decimal VatValue { get; }
        public decimal TotalValue { get; }
        public string PaymentTransactionId { get; }
        public DateTime IssuedDate { get; }
    }

    public record InvoiceFailed : IInvoiceIssuedEvent
    {
        internal InvoiceFailed(IEnumerable<string> reasons)
        {
            Reasons = reasons;
        }

        public IEnumerable<string> Reasons { get; }
    }

    public static IInvoiceIssuedEvent ToEvent(this IInvoice invoice, int invoiceId = 0) => invoice switch
    {
        IssuedInvoice issued => new InvoiceSucceeded(
            invoiceId,
            issued.BookingId,
            issued.InvoiceNumber.Value,
            issued.CustomerName.Value,
            issued.CustomerEmail.Value,
            issued.RoomType,
            issued.RoomNumber,
            issued.CheckInDate,
            issued.CheckOutDate,
            issued.NetValue.Value,
            issued.VatValue.Value,
            issued.TotalValue.Value,
            issued.PaymentTransactionId,
            issued.IssuedDate),
        InvalidInvoice invalid => new InvoiceFailed(invalid.Reasons),
        UnvalidatedInvoice _ => new InvoiceFailed(new[] { "Unexpected unvalidated state" }),
        ValidatedInvoice _ => new InvoiceFailed(new[] { "Unexpected validated state" }),
        CalculatedInvoice _ => new InvoiceFailed(new[] { "Unexpected calculated state" }),
        _ => new InvoiceFailed(new[] { $"Unexpected state: {invoice.GetType().Name}" })
    };
}


