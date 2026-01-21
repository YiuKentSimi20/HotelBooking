namespace Booking.Api.Models;

public class RoomAvailabilityResponse
{
    public int RoomId { get; set; }
    public string RoomNumber { get; set; }
    public string RoomType { get; set; }
    public bool IsClean { get; set; }
    public bool IsOutOfService { get; set; }
    public decimal PricePerNight { get; set; }
    public List<BookingPeriodInfo> BookedPeriods { get; set; } = new();
}

public class BookingPeriodInfo
{
    public int AssignmentId { get; set; }
    public int BookingId { get; set; }
    public string CheckInDate { get; set; }
    public string? CheckOutDate { get; set; }
}
