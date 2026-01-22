using FrontDesk.Domain.Models.ValueObjects;
using static FrontDesk.Domain.Models.Entities.CheckIn;

namespace FrontDesk.Domain.Operations;

internal sealed class GenerateAccessCodeOperation : CheckInOperation
{
    protected override ICheckIn OnValidated(ValidatedCheckIn checkIn)
    {
        // Generate a new access code for the guest
        var accessCode = AccessCode.Generate();

        return new AccessCodeGeneratedCheckIn(
            checkIn.BookingId,
            checkIn.CustomerName,
            checkIn.CustomerEmail,
            checkIn.RoomType,
            checkIn.RoomNumber,
            checkIn.CheckInDate,
            checkIn.CheckOutDate,
            checkIn.RoomId,
            accessCode);
    }
}
