namespace Application.Common.Exceptions;

/// <summary>
/// Represents an HTTP 401 Unauthorized error, typically thrown by infrastructure
/// or authentication layers for unexpected authentication failures.
/// Business logic should use <c>Result.Failure</c> instead.
/// </summary>
public sealed class UnauthorizedException : Exception
{
    public UnauthorizedException()
        : base("You are not authorized to access this resource.")
    {
    }

    public UnauthorizedException(string message)
        : base(message)
    {
    }

    public UnauthorizedException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

