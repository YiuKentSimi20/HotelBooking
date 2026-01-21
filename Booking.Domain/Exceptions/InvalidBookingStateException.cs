namespace Booking.Domain.Exceptions;

public class InvalidBookingStateException : DomainException
{
    public InvalidBookingStateException(string state) : base($"Invalid booking state: {state}") { }
}
