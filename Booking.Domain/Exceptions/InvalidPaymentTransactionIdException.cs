namespace Booking.Domain.Exceptions;

public class InvalidPaymentTransactionIdException : DomainException
{
    public InvalidPaymentTransactionIdException(string message) : base(message) { }
}
