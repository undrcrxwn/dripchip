using DripChip.Application.Abstractions.Identity;
using DripChip.Application.Exceptions;
using DripChip.Application.Models.Identity;
using DripChip.Domain.Constants;
using DripChip.Infrastructure.Identity.Extensions;
using DripChip.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;

namespace DripChip.Infrastructure.Identity.Services;

public class UserRepository : IUserRepository
{
    private readonly UserManager<User> _userManager;
    private readonly ApplicationDbContext _context;

    public UserRepository(UserManager<User> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public IQueryable<IUser> Users => _userManager.Users;

    public async Task<IUser?> FindByIdAsync(int userId) =>
        await _userManager.FindByIdAsync(userId.ToString());

    public async Task<IUser?> FindByEmailAsync(string email) =>
        await _userManager.FindByEmailAsync(email);

    public async Task<UserCreationResult> CreateAsync(string email, string password, string role, int? id = default)
    {
        var user = new User
        {
            Email = email,
            UserName = email,
            Role = role
        };

        IdentityResult result;
        if (id is null)
            result = await _userManager.CreateAsync(user, password);
        else
        {
            user.Id = id.Value;
            await _context.Users.AddAsync(user);
            result = await _userManager.SetPasswordAsync(user, password);
        }

        return result.Succeeded
            ? new UserCreationResult.Success(user)
            : new UserCreationResult.Failure(result.Errors.Select(error => error.Description));
    }

    public async Task DeleteAsync(int userId)
    {
        var user =
            await FindByIdAsync(userId)
            ?? throw new NotFoundException(nameof(User), userId);

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