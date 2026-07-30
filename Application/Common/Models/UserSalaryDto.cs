namespace Application.Common.Models;

/// <summary>
/// Data transfer object for a user's salary information.
/// </summary>
public sealed class UserSalaryDto
{
    public int UserId { get; set; }

    public decimal Salary { get; set; }
}

