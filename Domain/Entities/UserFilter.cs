namespace Domain.Entities;

public sealed record UserFilter
{
    public string? SearchTerm { get; init; }

    public string? Gender { get; init; }

    public bool? Active { get; init; }
}

