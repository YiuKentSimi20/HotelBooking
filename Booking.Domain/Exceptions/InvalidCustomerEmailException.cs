namespace Booking.Domain.Exceptions;

public class InvalidCustomerEmailException : DomainException
{
    public InvalidCustomerEmailException(string message) : base(message) { }
}
