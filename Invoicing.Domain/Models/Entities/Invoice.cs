using Invoicing.Domain.Models.Commands;
using Invoicing.Domain.Models.ValueObjects;

namespace Invoicing.Domain.Models.Entities;

public static class Invoice
{
    public interface IInvoice { }

    public record UnvalidatedInvoice : IInvoice
    {
        internal UnvalidatedInvoice(GenerateInvoiceCommand command)
        {
            Command = command;
        }

        public GenerateInvoiceCommand Command { get; }
    }

    public record ValidatedInvoice : IInvoice
    {
        internal ValidatedInvoice(
            int bookingId, 
            CustomerName customerName, 
            CustomerEmail customerEmail,
            string roomType,
            string roomNumber,
            DateTime checkInDate,
            DateTime checkOutDate,
            MoneyValue netValue,
            string paymentTransactionId)
        {
            BookingId = bookingId;
            CustomerName = customerName;
            CustomerEmail = customerEmail;
            RoomType = roomType;
            RoomNumber = roomNumber;
            CheckInDate = checkInDate;
            CheckOutDate = checkOutDate;
            NetValue = netValue;
            PaymentTransactionId = paymentTransactionId;
        }

        public int BookingId { get; }
        public CustomerName CustomerName { get; }
        public CustomerEmail CustomerEmail { get; }
        public string RoomType { get; }
        public string RoomNumber { get; }
        public DateTime CheckInDate { get; }
        public DateTime CheckOutDate { get; }
        public MoneyValue NetValue { get; }
        public string PaymentTransactionId { get; }
    }

    public record CalculatedInvoice : IInvoice
    {
        internal CalculatedInvoice(
            int bookingId, 
            CustomerName customerName, 
            CustomerEmail customerEmail,
            string roomType,
            string roomNumber,
            DateTime checkInDate,
            DateTime checkOutDate,
            MoneyValue netValue, 
            MoneyValue vatValue, 
            MoneyValue totalValue,
            string paymentTransactionId)
        {
            BookingId = bookingId;
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
        }

        public int BookingId { get; }
        public CustomerName CustomerName { get; }
        public CustomerEmail CustomerEmail { get; }
        public string RoomType { get; }
        public string RoomNumber { get; }
        public DateTime CheckInDate { get; }
        public DateTime CheckOutDate { get; }
        public MoneyValue NetValue { get; }
        public MoneyValue VatValue { get; }
        public MoneyValue TotalValue { get; }
        public string PaymentTransactionId { get; }
    }

    public record IssuedInvoice : IInvoice
    {
        internal IssuedInvoice(
            int bookingId, 
            InvoiceNumber invoiceNumber, 
            CustomerName customerName, 
            CustomerEmail customerEmail,
            string roomType,
            string roomNumber,
            DateTime checkInDate,
            DateTime checkOutDate,
            MoneyValue netValue, 
            MoneyValue vatValue, 
            MoneyValue totalValue,
            string paymentTransactionId,
            DateTime issuedDate)
        {
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

        public int BookingId { get; }
        public InvoiceNumber InvoiceNumber { get; }
        public CustomerName CustomerName { get; }
        public CustomerEmail CustomerEmail { get; }
        public string RoomType { get; }
        public string RoomNumber { get; }
        public DateTime CheckInDate { get; }
        public DateTime CheckOutDate { get; }
        public MoneyValue NetValue { get; }
        public MoneyValue VatValue { get; }
        public MoneyValue TotalValue { get; }
        public string PaymentTransactionId { get; }
        public DateTime IssuedDate { get; }
    }

    public record InvalidInvoice : IInvoice
    {
        internal InvalidInvoice(IEnumerable<string> reasons)
        {
            Reasons = reasons;
        }

        public IEnumerable<string> Reasons { get; }
    }
}


