namespace FrontDesk.Dto.Events;

public record CheckInCompletedIntegrationEvent
{
    public int AssignmentId { get; set; }
    public int BookingId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string RoomType { get; set; } = string.Empty;
    public string RoomNumber { get; set; } = string.Empty;
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int RoomId { get; set; }
    public string AccessCode { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }

    public override string ToString()
    {
        return $"CheckInCompletedIntegrationEvent {{ AssignmentId={AssignmentId}, BookingId={BookingId}, Customer={CustomerName}, Room={RoomType}/{RoomNumber}, RoomId={RoomId}, CheckIn={CheckInDate:yyyy-MM-dd}, CheckOut={CheckOutDate:yyyy-MM-dd}, AccessCode={AccessCode}, Timestamp={Timestamp:O} }}";
    }
}
