namespace Booking.Data.Models;

public class RoomEntity
{
    public int RoomId { get; set; }
    public string RoomNumber { get; set; }
    public string RoomType { get; set; }
    public bool IsClean { get; set; }
    public bool IsOutOfService { get; set; }
    public decimal PricePerNight { get; set; }
}
