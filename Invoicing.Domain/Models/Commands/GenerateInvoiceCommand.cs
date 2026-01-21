namespace Invoicing.Domain.Models.Commands;

public record GenerateInvoiceCommand(
    int BookingId,
    string CustomerName,
    string CustomerEmail,
    string RoomType,
    string RoomNumber,
    DateTime CheckInDate,
    DateTime CheckOutDate,
    decimal TotalAmount,
    string PaymentTransactionId,
    DateTime Timestamp
);
