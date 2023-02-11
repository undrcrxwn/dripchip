using DripChip.Application.Abstractions.Identity;
using DripChip.Infrastructure.Identity.Extensions;
using Microsoft.AspNetCore.Identity;

namespace DripChip.Infrastructure.Identity.Services;

public class UserService : IUserService
{
    public IQueryable<IUser> Users => _userManager.Users;

    private readonly UserManager<User> _userManager;

    public UserService(UserManager<User> userManager) =>
        _userManager = userManager;

    public async Task<IUser?> FindByIdAsync(int userId) =>
        await _userManager.FindByIdAsync(userId.ToString());

    public async Task<IUser?> FindByEmailAsync(string email) =>
        await _userManager.FindByEmailAsync(email);

    public async Task<IEnumerable<string>?> CreateAsync(string email, string password)
    {
        var user = new User
        {
            Email = email,
            UserName = email
        };
        
        var result = await _userManager.CreateAsync(user, password);
        return result.Errors.Select(x => x.Description);
    }

    public async Task DeleteAsync(int userId) =>
        await _userManager.DeleteAsync(new User { Id = userId });

    public async Task SetPasswordAsync(IUser user, string password) =>
        await _userManager.SetPasswordAsync((User)user, password);
}