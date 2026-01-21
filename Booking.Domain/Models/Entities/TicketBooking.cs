using Booking.Domain.Models.Commands;
using Booking.Domain.Models.ValueObjects;

namespace Booking.Domain.Models.Entities;

public static class TicketBooking
{
    public interface IBooking { }

    public record UnvalidatedBooking : IBooking
    {
        internal UnvalidatedBooking(BookTicketCommand command)
        {
            Command = command;
        }

        public BookTicketCommand Command { get; }
    }

    public record ValidatedBooking : IBooking
    {
        internal ValidatedBooking(CustomerName name, CustomerEmail email, RoomType roomType, DateOnly checkInDate, DateOnly checkOutDate)
        {
            CustomerName = name;
            CustomerEmail = email;
            RoomType = roomType;
            CheckInDate = checkInDate;
            CheckOutDate = checkOutDate;
        }

        public CustomerName CustomerName { get; }
        public CustomerEmail CustomerEmail { get; }
        public RoomType RoomType { get; }
        public DateOnly CheckInDate { get; }
        public DateOnly CheckOutDate { get; }
    }

    public record RoomAssignedBooking : IBooking
    {
        internal RoomAssignedBooking(CustomerName name, CustomerEmail email, RoomType roomType, DateOnly checkInDate, DateOnly checkOutDate, int roomId, string roomNumber, decimal pricePerNight)
        {
            CustomerName = name;
            CustomerEmail = email;
            RoomType = roomType;
            CheckInDate = checkInDate;
            CheckOutDate = checkOutDate;
            RoomId = roomId;
            RoomNumber = roomNumber;
            PricePerNight = pricePerNight;
        }

        public CustomerName CustomerName { get; }
        public CustomerEmail CustomerEmail { get; }
        public RoomType RoomType { get; }
        public DateOnly CheckInDate { get; }
        public DateOnly CheckOutDate { get; }
        public int RoomId { get; }
        public string RoomNumber { get; }
        public decimal PricePerNight { get; }
    }

    public record PriceCalculatedBooking : IBooking
    {
        internal PriceCalculatedBooking(CustomerName name, CustomerEmail email, RoomType roomType, DateOnly checkInDate, DateOnly checkOutDate, int roomId, string roomNumber, TotalAmount totalAmount)
        {
            CustomerName = name;
            CustomerEmail = email;
            RoomType = roomType;
            CheckInDate = checkInDate;
            CheckOutDate = checkOutDate;
            RoomId = roomId;
            RoomNumber = roomNumber;
            TotalAmount = totalAmount;
        }

        public CustomerName CustomerName { get; }
        public CustomerEmail CustomerEmail { get; }
        public RoomType RoomType { get; }
        public DateOnly CheckInDate { get; }
        public DateOnly CheckOutDate { get; }
        public int RoomId { get; }
        public string RoomNumber { get; }
        public TotalAmount TotalAmount { get; }
    }

    public record PaymentProcessedBooking : IBooking
    {
        internal PaymentProcessedBooking(
            CustomerName name,
            CustomerEmail email,
            RoomType roomType,
            DateOnly checkInDate,
            DateOnly checkOutDate,
            int roomId,
            string roomNumber,
            TotalAmount totalAmount,
            PaymentTransactionId transactionId)
        {
            CustomerName = name;
            CustomerEmail = email;
            RoomType = roomType;
            CheckInDate = checkInDate;
            CheckOutDate = checkOutDate;
            RoomId = roomId;
            RoomNumber = roomNumber;
            TotalAmount = totalAmount;
            PaymentTransactionId = transactionId;
        }

        public CustomerName CustomerName { get; }
        public CustomerEmail CustomerEmail { get; }
        public RoomType RoomType { get; }
        public DateOnly CheckInDate { get; }
        public DateOnly CheckOutDate { get; }
        public int RoomId { get; }
        public string RoomNumber { get; }
        public TotalAmount TotalAmount { get; }
        public PaymentTransactionId PaymentTransactionId { get; }
    }

    public record InvalidBooking : IBooking
    {
        internal InvalidBooking(IEnumerable<string> reasons)
        {
            Reasons = reasons;
        }

        public IEnumerable<string> Reasons { get; }
    }
}
