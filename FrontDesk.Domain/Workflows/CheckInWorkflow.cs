using FrontDesk.Domain.Models.Commands;
using FrontDesk.Domain.Models.Events;
using FrontDesk.Domain.Operations;
using FrontDesk.Domain.Repositories;
using static FrontDesk.Domain.Models.Entities.CheckIn;

namespace FrontDesk.Domain.Workflows;

public class CheckInWorkflow
{
    public async Task<CheckInCompletedEvent.ICheckInCompletedEvent> ExecuteAsync(
        CheckInCommand command,
        IRoomRepository roomRepository,
        IRoomAssignmentRepository assignmentRepository)
    {

        var unvalidated = new UnvalidatedCheckIn(command);

        ICheckIn result = new ValidateCheckInOperation(roomRepository).Transform(unvalidated);
        result = new GenerateAccessCodeOperation().Transform(result);

        if (result is AccessCodeGeneratedCheckIn accessCodeGenerated)
        {
            // Save room assignment
            var assignmentId = await assignmentRepository.SaveRoomAssignmentAsync(accessCodeGenerated);

            // Create completed check-in
            var completed = new CompletedCheckIn(
                accessCodeGenerated.BookingId,
                assignmentId,
                accessCodeGenerated.CustomerName,
                accessCodeGenerated.CustomerEmail,
                accessCodeGenerated.RoomType,
                accessCodeGenerated.RoomNumber,
                accessCodeGenerated.CheckInDate,
                accessCodeGenerated.CheckOutDate,
                accessCodeGenerated.RoomId,
                accessCodeGenerated.AccessCode);

            return completed.ToEvent(assignmentId);
        }

        return result.ToEvent();
    }
}
