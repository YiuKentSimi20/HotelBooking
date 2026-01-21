using Booking.Domain.Models.ValueObjects;
using static Booking.Domain.Models.Entities.TicketBooking;

namespace Booking.Domain.Operations;

internal sealed class CalculatePriceOperation : BookingOperation
{
    protected override IBooking OnRoomAssigned(RoomAssignedBooking booking)
    {
        //TODO : Calculate number of nights between CheckInDate and CheckOutDate
        
        // Calculate number of nights
        var nights = booking.CheckOutDate.DayNumber - booking.CheckInDate.DayNumber;
        
        // Calculate total amount: PricePerNight × nights
        var amount = booking.PricePerNight * nights;

        if (!TotalAmount.TryParse(amount, out var totalAmount))
            return new InvalidBooking(new[] { $"Calculated price {amount} is invalid" });

        return new PriceCalculatedBooking(
            booking.CustomerName,
            booking.CustomerEmail,
            booking.RoomType,
            booking.CheckInDate,
            booking.CheckOutDate,
            booking.RoomId,
            booking.RoomNumber,
            totalAmount!);
    }
}
