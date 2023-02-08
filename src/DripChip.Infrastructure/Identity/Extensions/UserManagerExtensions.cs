using Microsoft.AspNetCore.Identity;

namespace DripChip.Infrastructure.Identity.Extensions;

public static class UserManagerExtensions
{
    public static async Task<IdentityResult> SetPasswordAsync(this UserManager<Account> userManager, Account account, string newPassword)
    {
        var token = await userManager.GeneratePasswordResetTokenAsync(account);
        return await userManager.ResetPasswordAsync(account, token, newPassword);
    }
}