using static FrontDesk.Domain.Models.Entities.CheckIn;

namespace FrontDesk.Domain.Models.Events;

public static class CheckInCompletedEvent
{
    public interface ICheckInCompletedEvent { }

    public record CheckInSucceeded : ICheckInCompletedEvent
    {
        internal CheckInSucceeded(
            int assignmentId,
            int bookingId,
            string customerName,
            string customerEmail,
            string roomType,
            string roomNumber,
            DateTime checkInDate,
            DateTime checkOutDate,
            int roomId,
            string accessCode)
        {
            AssignmentId = assignmentId;
            BookingId = bookingId;
            CustomerName = customerName;
            CustomerEmail = customerEmail;
            RoomType = roomType;
            RoomNumber = roomNumber;
            CheckInDate = checkInDate;
            CheckOutDate = checkOutDate;
            RoomId = roomId;
            AccessCode = accessCode;
        }

        public int AssignmentId { get; }
        public int BookingId { get; }
        public string CustomerName { get; }
        public string CustomerEmail { get; }
        public string RoomType { get; }
        public string RoomNumber { get; }
        public DateTime CheckInDate { get; }
        public DateTime CheckOutDate { get; }
        public int RoomId { get; }
        public string AccessCode { get; }
    }

    public record CheckInFailed : ICheckInCompletedEvent
    {
        internal CheckInFailed(IEnumerable<string> reasons)
        {
            Reasons = reasons;
        }

        public IEnumerable<string> Reasons { get; }
    }

    public static ICheckInCompletedEvent ToEvent(this ICheckIn checkIn, int assignmentId = 0) => checkIn switch
    {
        CompletedCheckIn completed => new CheckInSucceeded(
            assignmentId,
            completed.BookingId,
            completed.CustomerName.Value,
            completed.CustomerEmail.Value,
            completed.RoomType,
            completed.RoomNumber.Value,
            completed.CheckInDate,
            completed.CheckOutDate,
            completed.RoomId,
            completed.AccessCode.Value),
        InvalidCheckIn invalid => new CheckInFailed(invalid.Reasons),
        UnvalidatedCheckIn _ => new CheckInFailed(new[] { "Unexpected unvalidated state" }),
        ValidatedCheckIn _ => new CheckInFailed(new[] { "Unexpected validated state" }),
        AccessCodeGeneratedCheckIn _ => new CheckInFailed(new[] { "Unexpected access-code-generated state" }),
        _ => new CheckInFailed(new[] { $"Unexpected state: {checkIn.GetType().Name}" })
    };
}
