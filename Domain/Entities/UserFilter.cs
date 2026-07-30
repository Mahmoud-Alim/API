namespace Domain.Entities;

/// <summary>
/// Filter object for querying users. Designed for easy extensibility —
/// add new filter properties without changing the repository interface signature.
/// </summary>
public sealed record UserFilter
{
    /// <summary>
    /// Optional general search term that searches across FirstName, LastName, and Email.
    /// </summary>
    public string? SearchTerm { get; init; }

    /// <summary>
    /// Optional gender filter (e.g., "Male", "Female").
    /// </summary>
    public string? Gender { get; init; }

    /// <summary>
    /// Optional active status filter. When null, no filtering by active status is applied.
    /// </summary>
    public bool? Active { get; init; }
}

