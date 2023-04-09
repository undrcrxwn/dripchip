using DripChip.Application.Abstractions.Identity;
using DripChip.Application.Exceptions;
using DripChip.Domain.Constants;
using Microsoft.AspNetCore.Identity;

namespace DripChip.Infrastructure.Identity;

public class User : IdentityUser<int>, IUser
{
    private string _role = Roles.User;

    public string Role
    {
        get => _role;
        set => _role = Roles.Contain(value)
            ? value
            : throw new ValidationException(nameof(Role), "The specified role is invalid.");
    }
}