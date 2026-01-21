namespace Booking.Domain.Exceptions;

public class InvalidCustomerNameException : DomainException
{
    public InvalidCustomerNameException(string message) : base(message) { }
}
