using DripChip.Application.Abstractions.Identity;

namespace DripChip.Application.Abstractions;

public interface IAuthenticationService
{
    public Task<IAccount?> AuthenticateAsync(string username, string password);
}