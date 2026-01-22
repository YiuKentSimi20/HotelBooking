namespace FrontDesk.Domain.Repositories;

public interface IRoomRepository
{
    Task<int?> GetRoomIdByNumberAsync(string roomNumber);
    Task<bool> IsRoomAvailableAsync(int roomId);
    Task MarkRoomAsOccupiedAsync(int roomId);
}
