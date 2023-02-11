using DripChip.Application.Abstractions;
using DripChip.Application.Abstractions.Identity;
using Microsoft.AspNetCore.Identity;

namespace DripChip.Infrastructure.Identity.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<User> _userManager;

    public AuthenticationService(UserManager<User> userManager) =>
        _userManager = userManager;
    
    public async Task<IUser?> AuthenticateAsync(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user is null)
            return null;

        return await _userManager.CheckPasswordAsync(user, password)
            ? user
            : null;
    }
}