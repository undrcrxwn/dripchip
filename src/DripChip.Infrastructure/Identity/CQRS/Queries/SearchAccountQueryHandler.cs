using DripChip.Application.Abstractions.Common;
using DripChip.Application.Extensions;
using DripChip.Application.Features.Accounts.Queries.Search;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DripChip.Infrastructure.Identity.CQRS.Queries;

public class SearchAccountQueryHandler : IRequestHandler<SearchAccountQuery, IEnumerable<SearchAccountResponse>>
{
    private readonly IFilterFactory _filterFactory;
    private readonly UserManager<Account> _userManager;

    public SearchAccountQueryHandler(IFilterFactory filterFactory, UserManager<Account> userManager)
    {
        _filterFactory = filterFactory;
        _userManager = userManager;
    }
    
    public async Task<IEnumerable<SearchAccountResponse>> Handle(SearchAccountQuery request, CancellationToken cancellationToken)
    {
        var accounts = _userManager.Users
            // Filtering
            .Where(_filterFactory.CreateCaseInsensitiveContainsFilter<Account>(x => x.FirstName, request.FirstName))
            .Where(_filterFactory.CreateCaseInsensitiveContainsFilter<Account>(x => x.LastName, request.LastName))
            .Where(_filterFactory.CreateCaseInsensitiveContainsFilter<Account>(x => x.Email!, request.Email))
            // Pagination
            .Skip(request.From)
            .Take(request.Size);

        return await accounts
            .ProjectToType<SearchAccountResponse>()
            .ToListAsync(cancellationToken);
    }
}