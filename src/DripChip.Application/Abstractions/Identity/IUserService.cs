namespace DripChip.Application.Abstractions.Identity;

public interface IUserService
{
    public IQueryable<IUser> Users { get; }

    public Task<IUser?> FindByIdAsync(int userId);
    public Task<IUser?> FindByEmailAsync(string email);

    /// <returns>Error descriptions if failed, otherwise null.</returns>
    public Task<IEnumerable<string>?> CreateAsync(string email, string password);

    public Task DeleteAsync(int userId);
    public Task DeleteAsync(IUser user);

    public Task SetUsernameAsync(IUser user, string username);
    public Task SetEmailAsync(IUser user, string email);
    public Task SetPasswordAsync(IUser user, string password);

    public Task SignOutAsync();
}