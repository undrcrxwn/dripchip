using DripChip.Application.Abstractions;
using DripChip.Application.Abstractions.Identity;
using Microsoft.AspNetCore.Identity;

namespace DripChip.Infrastructure.Identity.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<Account> _userManager;

    public AuthenticationService(UserManager<Account> userManager) =>
        _userManager = userManager;
    
    public async Task<IAccount?> AuthenticateAsync(string username, string password)
    {
        var account = await _userManager.FindByNameAsync(username);
        if (account is null)
            return null;

        return await _userManager.CheckPasswordAsync(account, password)
            ? account
            : null;
    }
}