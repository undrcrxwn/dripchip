using System.Security.Claims;
using DripChip.Application.Abstractions;

namespace DripChip.Api.Services;

public class CurrentUserProvider : ICurrentUserProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserProvider(IHttpContextAccessor httpContextAccessor) =>
        _httpContextAccessor = httpContextAccessor;

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
}