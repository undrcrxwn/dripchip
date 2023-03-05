using DripChip.Application.Abstractions.Identity;
using DripChip.Application.Exceptions;
using DripChip.Infrastructure.Identity.Extensions;
using Microsoft.AspNetCore.Identity;

namespace DripChip.Infrastructure.Identity.Services;

public class UserRepository : IUserRepository
{
    private readonly UserManager<User> _userManager;
    
    public UserRepository(UserManager<User> userManager) =>
        _userManager = userManager;

    public IQueryable<IUser> Users => _userManager.Users;
    
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
        return result.Succeeded
            ? null
            : result.Errors.Select(x => x.Description);
    }

    public async Task DeleteAsync(int userId)
    {
        var user =
            await FindByIdAsync(userId)
            ?? throw new NotFoundException();

        await DeleteAsync(user);
    }

    public async Task DeleteAsync(IUser user) =>
        await _userManager.DeleteAsync((User)user);

    public async Task SetUsernameAsync(IUser user, string username) =>
        await _userManager.SetUserNameAsync((User)user, username);

    public async Task SetEmailAsync(IUser user, string email) =>
        await _userManager.SetEmailAsync((User)user, email);

    public async Task SetPasswordAsync(IUser user, string password) =>
        await _userManager.SetPasswordAsync((User)user, password);
}