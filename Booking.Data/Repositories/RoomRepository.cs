using System.Linq;
using Booking.Domain.Models.ValueObjects;
using Booking.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Booking.Data.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly BookingDbContext _context;

    public RoomRepository(BookingDbContext context)
    {
        _context = context;
    }

    public async Task<bool> IsRoomAvailableAsync(RoomType roomType, DateOnly checkInDate, DateOnly checkOutDate)
    {
        var availableRoomId = await GetAvailableRoomIdAsync(roomType, checkInDate, checkOutDate);
        return availableRoomId.HasValue;
    }

    public async Task<int?> GetAvailableRoomIdAsync(RoomType roomType, DateOnly checkInDate, DateOnly checkOutDate)
    {
        // Get all rooms of the requested type that are in service
        var roomsOfType = await _context.Rooms
            .Where(r => r.RoomType == roomType.Value && !r.IsOutOfService)
            .Select(r => r.RoomId)
            .ToListAsync();

        if (roomsOfType.Count == 0)
        {
            return null;
        }

        // Convert DateOnly to DateTime for comparison
        var checkInDateTime = checkInDate.ToDateTime(TimeOnly.MinValue);
        var checkOutDateTime = checkOutDate.ToDateTime(TimeOnly.MinValue);

        // Get all room IDs that have conflicting assignments
        // A room is occupied if CheckOutDate is NULL (still checked in) or if there's a date overlap
        var conflictingRoomIds = await _context.RoomAssignments
            .Where(ra => roomsOfType.Contains(ra.RoomId))
            .Where(ra => ra.CheckOutDate == null || 
                        (ra.CheckInDate < checkOutDateTime && ra.CheckOutDate > checkInDateTime))
            .Select(ra => ra.RoomId)
            .Distinct()
            .ToListAsync();

        // Find a room that's not in the conflicting list
        foreach (var roomId in roomsOfType)
        {
            if (!conflictingRoomIds.Contains(roomId))
            {
                return roomId;
            }
        }

        return null;
    }

    public async Task<(int RoomId, string RoomNumber, decimal PricePerNight)?> GetAvailableRoomAsync(RoomType roomType, DateOnly checkInDate, DateOnly checkOutDate)
    {
        // Get all rooms of the requested type that are in service
        var roomsOfType = await _context.Rooms
            .Where(r => r.RoomType == roomType.Value && !r.IsOutOfService)
            .Select(r => new { r.RoomId, r.RoomNumber, r.PricePerNight })
            .ToListAsync();

        if (roomsOfType.Count == 0)
        {
            return null;
        }

        // Convert DateOnly to DateTime for comparison
        var checkInDateTime = checkInDate.ToDateTime(TimeOnly.MinValue);
        var checkOutDateTime = checkOutDate.ToDateTime(TimeOnly.MinValue);

        // Get all room IDs that have conflicting assignments
        var conflictingRoomIds = await _context.RoomAssignments
            .Where(ra => roomsOfType.Select(r => r.RoomId).Contains(ra.RoomId))
            .Where(ra => ra.CheckOutDate == null || 
                        (ra.CheckInDate < checkOutDateTime && ra.CheckOutDate > checkInDateTime))
            .Select(ra => ra.RoomId)
            .Distinct()
            .ToListAsync();

        // Find a room that's not in the conflicting list
        var availableRoom = roomsOfType.FirstOrDefault(r => !conflictingRoomIds.Contains(r.RoomId));
        
        if (availableRoom != null)
        {
            return (availableRoom.RoomId, availableRoom.RoomNumber, availableRoom.PricePerNight);
        }

        return null;
    }

    public async Task<IEnumerable<RoomWithAssignments>> GetAllRoomsWithAssignmentsAsync()
    {
        var rooms = await _context.Rooms.ToListAsync();
        var assignments = await _context.RoomAssignments.ToListAsync();

        return rooms.Select(room => new RoomWithAssignments
        {
            RoomId = room.RoomId,
            RoomNumber = room.RoomNumber,
            RoomType = room.RoomType,
            IsClean = room.IsClean,
            IsOutOfService = room.IsOutOfService,
            PricePerNight = room.PricePerNight,
            Assignments = assignments
                .Where(a => a.RoomId == room.RoomId)
                .Select(a => new RoomAssignmentInfo
                {
                    AssignmentId = a.AssignmentId,
                    BookingId = a.BookingId,
                    CheckInDate = a.CheckInDate,
                    CheckOutDate = a.CheckOutDate
                })
                .OrderBy(a => a.CheckInDate)
                .ToList()
        }).ToList();
    }
}


