namespace Application.Common.Models;

public sealed class Result<T>
{
    private Result(T data)
    {
        Data = data;
        IsSuccess = true;
        Error = null;
        StatusCode = GetDefaultSuccessStatusCode();
    }

    private Result(string error, int statusCode)
    {
        Data = default;
        IsSuccess = false;
        Error = error;
        StatusCode = statusCode;
    }

    public T? Data { get; }

    public bool IsSuccess { get; }

    public string? Error { get; }

    public int StatusCode { get; }
    public bool IsFailure => !IsSuccess;

    public static Result<T> Success(T data) => new(data);

    public static Result<T> Failure(string error, int statusCode = 400) => new(error, statusCode);

    public static Result<T> NotFound(string error) => new(error, 404);

    private static int GetDefaultSuccessStatusCode() => 200;
}

