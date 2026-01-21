namespace Booking.Data.Models;

public class RoomAssignmentEntity
{
    public int AssignmentId { get; set; }
    public int BookingId { get; set; }
    public int RoomId { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime? CheckOutDate { get; set; }
}
