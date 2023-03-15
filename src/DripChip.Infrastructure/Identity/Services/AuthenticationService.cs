using DripChip.Application.Abstractions.Identity;
using DripChip.Application.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace DripChip.Infrastructure.Identity.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<User> _userManager;

    public AuthenticationService(UserManager<User> userManager) =>
        _userManager = userManager;
    
    public async Task<AuthenticationResult> AuthenticateAsync(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user is null)
            return new AuthenticationResult.Failure("The specified username is invalid.");

        return await _userManager.CheckPasswordAsync(user, password)
            ? new AuthenticationResult.Success(user)
            : new AuthenticationResult.Failure("The specified password is invalid.");
    }
}