namespace Invoicing.Data.Models;

public class InvoiceEntity
{
    public int InvoiceId { get; set; }
    public int BookingId { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string RoomType { get; set; } = string.Empty;
    public string RoomNumber { get; set; } = string.Empty;
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public decimal NetValue { get; set; }
    public decimal VatValue { get; set; }
    public decimal TotalValue { get; set; }
    public string PaymentTransactionId { get; set; } = string.Empty;
    public DateTime IssuedDate { get; set; }
}


