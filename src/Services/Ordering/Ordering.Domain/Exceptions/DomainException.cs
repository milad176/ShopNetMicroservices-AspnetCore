namespace Ordering.Domain.Exceptions;

public class DomainException : Exception
{
    public DomainException(string message) : base($"Domain exceptions: {message} throws from domain layer.")
    {
    }
}