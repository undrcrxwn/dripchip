using DripChip.Application.Models.Identity;

namespace DripChip.Application.Abstractions.Identity;

/// <summary>
/// Authentication service abstraction declaring infrastructure-dependent identity concerns.
/// </summary>
public interface IAuthenticationService
{
    /// <summary>
    /// Authenticates user by credentials.
    /// </summary>
    /// <returns>
    /// Authentication result: <see cref="AuthenticationResult.Success"/> with <see cref="IUser"/>
    /// or <see cref="AuthenticationResult.Failure"/> with reason.
    /// </returns>
    public Task<AuthenticationResult> AuthenticateAsync(string username, string password);
}