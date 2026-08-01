namespace Application.Common.Models;

public sealed class UserJobInfoDto
{    public int UserId { get; set; }

    public string JobTitle { get; set; } = string.Empty;

    public string Department { get; set; } = string.Empty;
}

