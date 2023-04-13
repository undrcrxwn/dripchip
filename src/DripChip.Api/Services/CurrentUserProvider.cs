using System.Security.Claims;
using DripChip.Application.Abstractions;
using DripChip.Application.Abstractions.Persistence;
using DripChip.Domain.Entities;

namespace DripChip.Api.Services;

/// <summary>
/// Provides brief information containing unique identifier and authentication data related to the request issuer.
/// </summary>
internal class CurrentUserProvider : ICurrentUserProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IApplicationDbContext _context;

    public CurrentUserProvider(IHttpContextAccessor httpContextAccessor, IApplicationDbContext context)
    {
        _httpContextAccessor = httpContextAccessor;
        _context = context;
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

    public bool IsAuthenticated => AccountId is not null;

    public async Task<Account?> GetAccountAsync() => AccountId is not null
        ? await _context.Accounts.FindAsync(AccountId)
        : null;
}