using DripChip.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace DripChip.Infrastructure.Identity.Extensions;

public static class UserManagerExtensions
{
    public static async Task<IdentityResult> SetPasswordAsync(this UserManager<Account> userManager, Account user, string newPassword)
    {
        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        return await userManager.ResetPasswordAsync(user, token, newPassword);
    }
}