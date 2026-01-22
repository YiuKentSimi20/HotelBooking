namespace FrontDesk.Data.Models;

public class RoomEntity
{
    public int RoomId { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public string RoomType { get; set; } = string.Empty;
    public bool IsClean { get; set; }
    public bool IsOutOfService { get; set; }
}
