namespace Application.Common.Models;

/// <summary>
/// Data transfer object for a user's job information.
/// </summary>
public sealed class UserJobInfoDto
{    public int UserId { get; set; }

    public string JobTitle { get; set; } = string.Empty;

    public string Department { get; set; } = string.Empty;
}

