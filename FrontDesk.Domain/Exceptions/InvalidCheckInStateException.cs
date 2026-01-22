namespace FrontDesk.Domain.Exceptions;

public class InvalidCheckInStateException : DomainException
{
    public InvalidCheckInStateException(string stateName) 
        : base($"Invalid check-in state: {stateName}") { }
}
