using HotelBooking.Events;

namespace Invoicing.Dto.Events;

public record BookingPaidIntegrationEvent
{
    public int BookingId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string RoomType { get; set; } = string.Empty;
    public string RoomNumber { get; set; } = string.Empty;
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string PaymentTransactionId { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    
}