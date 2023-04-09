using System.Security.Claims;
using DripChip.Application.Abstractions;
using DripChip.Application.Abstractions.Identity;
using DripChip.Application.Abstractions.Persistence;

namespace DripChip.Api.Services;

/// <summary>
/// Provides brief information containing unique identifier and authentication data related to the request issuer.
/// </summary>
internal class CurrentUserProvider : ICurrentUserProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRepository _users;

    public CurrentUserProvider(IHttpContextAccessor httpContextAccessor, IUserRepository users)
    {
        _httpContextAccessor = httpContextAccessor;
        _users = users;
    }

    public int? AccountId
    {
        get
        {
            var claimValue = _httpContextAccessor.HttpContext?.User
                .FindFirstValue(ClaimTypes.NameIdentifier);

            var success = int.TryParse(claimValue, out var accountId);
            return success ? accountId : null;
        }
    }

    public bool BypassAuthentication { get; set; }

    public bool IsAuthenticated => AccountId is not null;

    public async Task<IUser?> GetUserAsync() => AccountId is { } id
        ? await _users.FindByIdAsync(id)
        : null;
}