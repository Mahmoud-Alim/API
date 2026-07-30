namespace Application.Features.Users.GetUserById;

public sealed class GetUserByIdResponse
{
    public int UserId { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Gender { get; set; } = string.Empty;

    public bool Active { get; set; }
}

