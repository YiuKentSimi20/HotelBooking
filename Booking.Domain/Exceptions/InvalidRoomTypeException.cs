namespace Booking.Domain.Exceptions;

public class InvalidRoomTypeException : DomainException
{
    public InvalidRoomTypeException(string message) : base(message) { }
}
