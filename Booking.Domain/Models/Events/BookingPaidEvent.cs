using static Booking.Domain.Models.Entities.TicketBooking;

namespace Booking.Domain.Models.Events;

public static class BookingPaidEvent
{
    public interface IBookingPaidEvent { }

    public record BookingSucceeded : IBookingPaidEvent
    {
        internal BookingSucceeded(
            int bookingId,
            string customerName,
            string customerEmail,
            string roomType,
            string roomNumber,
            DateOnly checkInDate,
            DateOnly checkOutDate,
            decimal totalAmount,
            string paymentTransactionId)
        {
            BookingId = bookingId;
            CustomerName = customerName;
            CustomerEmail = customerEmail;
            RoomType = roomType;
            CheckInDate = checkInDate;
            CheckOutDate = checkOutDate;
            TotalAmount = totalAmount;
            PaymentTransactionId = paymentTransactionId;
            RoomNumber = roomNumber;
        }

        public int BookingId { get; }
        public string CustomerName { get; }
        public string CustomerEmail { get; }
        public string RoomType { get; }
        public string RoomNumber { get; }
        public DateOnly CheckInDate { get; }
        public DateOnly CheckOutDate { get; }
        public decimal TotalAmount { get; }
        public string PaymentTransactionId { get; }
    }

    public record BookingFailed : IBookingPaidEvent
    {
        internal BookingFailed(IEnumerable<string> reasons)
        {
            Reasons = reasons;
        }

        public IEnumerable<string> Reasons { get; }
    }

    public static IBookingPaidEvent ToEvent(this IBooking booking, int bookingId = 0) => booking switch
    {
        PaymentProcessedBooking processed => new BookingSucceeded(
            bookingId,
            processed.CustomerName.Value,
            processed.CustomerEmail.Value,
            processed.RoomType.Value,
            processed.RoomNumber,
            processed.CheckInDate,
            processed.CheckOutDate,
            processed.TotalAmount.Value,
            processed.PaymentTransactionId.Value),
        InvalidBooking invalid => new BookingFailed(invalid.Reasons),
        UnvalidatedBooking _ => new BookingFailed(new[] { "Unexpected unvalidated state" }),
        ValidatedBooking _ => new BookingFailed(new[] { "Unexpected validated state" }),
        RoomAssignedBooking _ => new BookingFailed(new[] { "Unexpected room-assigned state" }),
        PriceCalculatedBooking _ => new BookingFailed(new[] { "Unexpected price-calculated state" }),
        _ => new BookingFailed(new[] { $"Unexpected state: {booking.GetType().Name}" })
    };
}
