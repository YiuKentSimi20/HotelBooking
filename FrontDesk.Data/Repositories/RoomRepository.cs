using FrontDesk.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FrontDesk.Data.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly FrontDeskDbContext _context;

    public RoomRepository(FrontDeskDbContext context)
    {
        _context = context;
    }

    public async Task<int?> GetRoomIdByNumberAsync(string roomNumber)
    {
        var room = await _context.Rooms
            .FirstOrDefaultAsync(r => r.RoomNumber == roomNumber);
        
        return room?.RoomId;
    }

    public async Task<bool> IsRoomAvailableAsync(int roomId)
    {
        var room = await _context.Rooms.FindAsync(roomId);
        return room != null && !room.IsOutOfService;
    }

    public async Task MarkRoomAsOccupiedAsync(int roomId)
    {
        var room = await _context.Rooms.FindAsync(roomId);
        if (room != null)
        {
            // Room is now occupied - no change to IsOutOfService needed
            // The RoomAssignment entry tracks occupancy
            await _context.SaveChangesAsync();
        }
    }

    public async Task MarkRoomAsDirtyAsync(int roomId)
    {
        var room = await _context.Rooms.FindAsync(roomId);
        if (room != null)
        {
            room.IsClean = false;
            await _context.SaveChangesAsync();
        }
    }
}
