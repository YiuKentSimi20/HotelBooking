using FrontDesk.Domain.Models.Commands;
using FrontDesk.Domain.Models.ValueObjects;

namespace FrontDesk.Domain.Models.Entities;

public static class CheckIn
{
    public interface ICheckIn { }

    public record UnvalidatedCheckIn : ICheckIn
    {
        internal UnvalidatedCheckIn(CheckInCommand command)
        {
            Command = command;
        }

        public CheckInCommand Command { get; }
    }

    public record ValidatedCheckIn : ICheckIn
    {
        internal ValidatedCheckIn(
            int bookingId,
            CustomerName customerName,
            CustomerEmail customerEmail,
            string roomType,
            RoomNumber roomNumber,
            DateTime checkInDate,
            DateTime checkOutDate,
            int roomId)
        {
            BookingId = bookingId;
            CustomerName = customerName;
            CustomerEmail = customerEmail;
            RoomType = roomType;
            RoomNumber = roomNumber;
            CheckInDate = checkInDate;
            CheckOutDate = checkOutDate;
            RoomId = roomId;
        }

        public int BookingId { get; }
        public CustomerName CustomerName { get; }
        public CustomerEmail CustomerEmail { get; }
        public string RoomType { get; }
        public RoomNumber RoomNumber { get; }
        public DateTime CheckInDate { get; }
        public DateTime CheckOutDate { get; }
        public int RoomId { get; }
    }

    public record AccessCodeGeneratedCheckIn : ICheckIn
    {
        internal AccessCodeGeneratedCheckIn(
            int bookingId,
            CustomerName customerName,
            CustomerEmail customerEmail,
            string roomType,
            RoomNumber roomNumber,
            DateTime checkInDate,
            DateTime checkOutDate,
            int roomId,
            AccessCode accessCode)
        {
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

        public int BookingId { get; }
        public CustomerName CustomerName { get; }
        public CustomerEmail CustomerEmail { get; }
        public string RoomType { get; }
        public RoomNumber RoomNumber { get; }
        public DateTime CheckInDate { get; }
        public DateTime CheckOutDate { get; }
        public int RoomId { get; }
        public AccessCode AccessCode { get; }
    }

    public record CompletedCheckIn : ICheckIn
    {
        internal CompletedCheckIn(
            int bookingId,
            int assignmentId,
            CustomerName customerName,
            CustomerEmail customerEmail,
            string roomType,
            RoomNumber roomNumber,
            DateTime checkInDate,
            DateTime checkOutDate,
            int roomId,
            AccessCode accessCode)
        {
            BookingId = bookingId;
            AssignmentId = assignmentId;
            CustomerName = customerName;
            CustomerEmail = customerEmail;
            RoomType = roomType;
            RoomNumber = roomNumber;
            CheckInDate = checkInDate;
            CheckOutDate = checkOutDate;
            RoomId = roomId;
            AccessCode = accessCode;
        }

        public int BookingId { get; }
        public int AssignmentId { get; }
        public CustomerName CustomerName { get; }
        public CustomerEmail CustomerEmail { get; }
        public string RoomType { get; }
        public RoomNumber RoomNumber { get; }
        public DateTime CheckInDate { get; }
        public DateTime CheckOutDate { get; }
        public int RoomId { get; }
        public AccessCode AccessCode { get; }
    }

    public record InvalidCheckIn : ICheckIn
    {
        internal InvalidCheckIn(IEnumerable<string> reasons)
        {
            Reasons = reasons;
        }

        public IEnumerable<string> Reasons { get; }
    }
}
