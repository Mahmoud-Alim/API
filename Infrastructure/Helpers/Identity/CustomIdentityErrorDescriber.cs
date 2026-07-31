using Microsoft.AspNetCore.Identity;

namespace Infrastructure.IdentityHelper;

public class CustomIdentityErrorDescriber : IdentityErrorDescriber
{
    public override IdentityError DuplicateEmail(string email)
    {
        return new IdentityError
        {
            Code = "DuplicateEmail",
            Description = "This email is already registered."
        };
    }

    public override IdentityError DuplicateUserName(string userName)
    {
        return new IdentityError
        {
            Code = "DuplicateUserName",
            Description = "This username is already taken."
        };
    }
}