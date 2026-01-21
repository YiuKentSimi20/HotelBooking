using Booking.Domain.Models.ValueObjects;
using static Booking.Domain.Models.Entities.TicketBooking;

namespace Booking.Domain.Operations;

internal sealed class ProcessPaymentOperation : BookingOperation
{
    private readonly Func<string, decimal, string> processPayment;

    internal ProcessPaymentOperation(Func<string, decimal, string> processPayment)
    {
        this.processPayment = processPayment;
    }

    protected override IBooking OnPriceCalculated(PriceCalculatedBooking booking)
    {
        var transactionId = processPayment(booking.CustomerEmail.Value, booking.TotalAmount.Value);

        if (!PaymentTransactionId.TryParse(transactionId, out var txnId))
            return new InvalidBooking(new[] { $"Payment transaction ID '{transactionId}' is invalid" });

        return new PaymentProcessedBooking(
            booking.CustomerName,
            booking.CustomerEmail,
            booking.RoomType,
            booking.CheckInDate,
            booking.CheckOutDate,
            booking.RoomId,
            booking.RoomNumber,
            booking.TotalAmount,
            txnId!);
    }
}
