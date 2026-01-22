using static FrontDesk.Domain.Models.Entities.CheckIn;

namespace FrontDesk.Domain.Repositories;

public interface IRoomAssignmentRepository
{
    Task<int> SaveRoomAssignmentAsync(AccessCodeGeneratedCheckIn checkIn);
}
