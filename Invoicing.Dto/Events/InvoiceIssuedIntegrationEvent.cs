namespace Invoicing.Dto.Events;

public record InvoiceIssuedIntegrationEvent
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
    public DateTime Timestamp { get; set; }

    public override string ToString()
    {
        return $"InvoiceIssuedIntegrationEvent {{ InvoiceId={InvoiceId}, BookingId={BookingId}, InvoiceNumber={InvoiceNumber}, Customer={CustomerName}, Room={RoomType}/{RoomNumber}, CheckIn={CheckInDate:yyyy-MM-dd}, CheckOut={CheckOutDate:yyyy-MM-dd}, Net={NetValue:0.00}, VAT={VatValue:0.00}, Total={TotalValue:0.00}, PaymentTxn={PaymentTransactionId}, IssuedDate={IssuedDate:O} }}";
    }
}