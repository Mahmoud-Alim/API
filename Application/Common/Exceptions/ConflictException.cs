namespace Application.Common.Exceptions;

public sealed class ConflictException : Exception
{
    public ConflictException()
        : base("The request could not be completed due to a conflict with the current state of the resource.")
    {
    }

    public ConflictException(string message)
        : base(message)
    {
    }

    public ConflictException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

