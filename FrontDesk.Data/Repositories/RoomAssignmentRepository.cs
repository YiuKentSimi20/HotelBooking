using FrontDesk.Data.Models;
using FrontDesk.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using static FrontDesk.Domain.Models.Entities.CheckIn;

namespace FrontDesk.Data.Repositories;

public class RoomAssignmentRepository : IRoomAssignmentRepository
{
    private readonly FrontDeskDbContext _context;

    public RoomAssignmentRepository(FrontDeskDbContext context)
    {
        _context = context;
    }

    public async Task<int> SaveRoomAssignmentAsync(AccessCodeGeneratedCheckIn checkIn)
    {
        var entity = new RoomAssignmentEntity
        {
            BookingId = checkIn.BookingId,
            RoomId = checkIn.RoomId,
            CheckInDate = checkIn.CheckInDate,
            CheckOutDate = checkIn.CheckOutDate,
            AccessCode = checkIn.AccessCode.Value
        };

        _context.RoomAssignments.Add(entity);
        await _context.SaveChangesAsync();

        return entity.AssignmentId;
    }
    
}
