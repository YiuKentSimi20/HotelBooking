namespace Booking.Domain.Exceptions;

public class InvalidTotalAmountException : DomainException
{
    public InvalidTotalAmountException(string message) : base(message) { }
}
