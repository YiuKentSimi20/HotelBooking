namespace Invoicing.Domain.Exceptions;

public class InvalidInvoiceStateException : DomainException
{
    public InvalidInvoiceStateException(string stateName) 
        : base($"Invalid invoice state: {stateName}") { }
}
