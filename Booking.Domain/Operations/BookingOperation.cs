using Booking.Domain.Exceptions;
using static Booking.Domain.Models.Entities.TicketBooking;

namespace Booking.Domain.Operations;

public abstract class BookingOperation : DomainOperation<IBooking, object, IBooking>
{
    internal IBooking Transform(IBooking booking) => Transform(booking, null);

    public override IBooking Transform(IBooking booking, object? state) => booking switch
    {
        UnvalidatedBooking unvalidated => OnUnvalidated(unvalidated),
        ValidatedBooking validated => OnValidated(validated),
        RoomAssignedBooking roomAssigned => OnRoomAssigned(roomAssigned),
        PriceCalculatedBooking priced => OnPriceCalculated(priced),
        PaymentProcessedBooking processed => OnPaymentProcessed(processed),
        InvalidBooking invalid => OnInvalid(invalid),
        _ => throw new InvalidBookingStateException(booking.GetType().Name)
    };

    protected virtual IBooking OnUnvalidated(UnvalidatedBooking booking) => booking;
    protected virtual IBooking OnValidated(ValidatedBooking booking) => booking;
    protected virtual IBooking OnRoomAssigned(RoomAssignedBooking booking) => booking;
    protected virtual IBooking OnPriceCalculated(PriceCalculatedBooking booking) => booking;
    protected virtual IBooking OnPaymentProcessed(PaymentProcessedBooking booking) => booking;
    protected virtual IBooking OnInvalid(InvalidBooking booking) => booking;
}
