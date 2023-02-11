using DripChip.Application.Abstractions.Identity;

namespace DripChip.Application.Abstractions;

public interface IAuthenticationService
{
    /// <summary>
    /// Authenticates user by credentials.
    /// </summary>
    /// <returns>User ID if the specified credentials are valid, otherwise null.</returns>
    public Task<IUser?> AuthenticateAsync(string username, string password);
}