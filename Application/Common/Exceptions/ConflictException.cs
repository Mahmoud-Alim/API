namespace Application.Common.Exceptions;

/// <summary>
/// Represents an HTTP 409 Conflict error, typically thrown by infrastructure
/// or data layers for unexpected concurrency/conflict scenarios.
/// Business logic should use <c>Result.Failure</c> instead.
/// </summary>
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

