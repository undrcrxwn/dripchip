using DripChip.Application.Abstractions.Identity;
using DripChip.Domain.Abstractions;

namespace DripChip.Application.Abstractions;

public interface IAuthenticationService
{
    public Task<IAccount?> AuthenticateAsync(string username, string password);
}