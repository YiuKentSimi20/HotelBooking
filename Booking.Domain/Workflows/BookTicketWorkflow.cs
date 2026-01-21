using Booking.Domain.Models.Commands;
using Booking.Domain.Models.Events;
using Booking.Domain.Operations;
using Booking.Domain.Repositories;
using static Booking.Domain.Models.Entities.TicketBooking;

namespace Booking.Domain.Workflows;

public class BookTicketWorkflow
{
    public async Task<BookingPaidEvent.IBookingPaidEvent> ExecuteAsync(
        BookTicketCommand command,
        Func<string, decimal, string> processPayment,
        IBookingRepository repository,
        IRoomRepository roomRepository)
    {
        var unvalidated = new UnvalidatedBooking(command);

        IBooking result = new ValidateBookingOperation(roomRepository).Transform(unvalidated);
        result = new AssignRoomOperation(roomRepository).Transform(result);
        result = new CalculatePriceOperation().Transform(result);
        result = new ProcessPaymentOperation(processPayment).Transform(result);

        if (result is PaymentProcessedBooking processed)
        {
            // Save booking with room number (already assigned in the booking state)
            var bookingId = await repository.SaveBookingAsync(processed, processed.RoomNumber);
            
            // Create room assignment record
            await repository.AssignRoomToBookingAsync(bookingId, processed.RoomId, processed.CheckInDate, processed.CheckOutDate);
            
            return result.ToEvent(bookingId);
        }

        return result.ToEvent();
    }
}
