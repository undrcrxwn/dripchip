namespace DripChip.Application.Abstractions.Identity;

/// <summary>
/// Authentication service abstraction declaring infrastructure-dependent identity concerns.
/// </summary>
public interface IAuthenticationService
{
    /// <summary>
    /// Authenticates user by credentials.
    /// </summary>
    /// <returns>User ID if the specified credentials are valid, otherwise null.</returns>
    public Task<IUser?> AuthenticateAsync(string username, string password);
}