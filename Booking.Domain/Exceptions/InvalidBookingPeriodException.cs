namespace Booking.Domain.Exceptions;

public class InvalidBookingPeriodException : DomainException
{
    public InvalidBookingPeriodException(string message) : base(message) { }
}
