namespace FrontDesk.Domain.Models.Commands;

public record CheckInCommand(
    int InvoiceId,
    int BookingId,
    string InvoiceNumber,
    string CustomerName,
    string CustomerEmail,
    string RoomType,
    string RoomNumber,
    DateTime CheckInDate,
    DateTime CheckOutDate,
    decimal NetValue,
    decimal VatValue,
    decimal TotalValue,
    string PaymentTransactionId,
    DateTime IssuedDate,
    DateTime Timestamp
);
