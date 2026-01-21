using Booking.Domain.Models.ValueObjects;

namespace Booking.Domain.Repositories;

public interface IRoomRepository
{
    Task<bool> IsRoomAvailableAsync(RoomType roomType, DateOnly checkInDate, DateOnly checkOutDate);
    Task<int?> GetAvailableRoomIdAsync(RoomType roomType, DateOnly checkInDate, DateOnly checkOutDate);
    Task<(int RoomId, string RoomNumber, decimal PricePerNight)?> GetAvailableRoomAsync(RoomType roomType, DateOnly checkInDate, DateOnly checkOutDate);
    Task<IEnumerable<RoomWithAssignments>> GetAllRoomsWithAssignmentsAsync();
}

public record RoomWithAssignments
{
    public int RoomId { get; init; }
    public string RoomNumber { get; init; }
    public string RoomType { get; init; }
    public bool IsClean { get; init; }
    public bool IsOutOfService { get; init; }
    public decimal PricePerNight { get; init; }
    public IEnumerable<RoomAssignmentInfo> Assignments { get; init; }
}

public record RoomAssignmentInfo
{
    public int AssignmentId { get; init; }
    public int BookingId { get; init; }
    public DateTime CheckInDate { get; init; }
    public DateTime? CheckOutDate { get; init; }
}
