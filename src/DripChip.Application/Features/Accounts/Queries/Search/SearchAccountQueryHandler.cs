using DripChip.Application.Abstractions.Filtering;
using DripChip.Application.Abstractions.Identity;
using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Extensions;
using DripChip.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DripChip.Application.Features.Accounts.Queries.Search;

public class SearchAccountQueryHandler : IRequestHandler<SearchAccountQuery, IEnumerable<SearchAccountResponse>>
{
    private readonly IFilterFactory _filterFactory;
    private readonly IUserService _users;
    private readonly IApplicationDbContext _context;

    public SearchAccountQueryHandler(IFilterFactory filterFactory, IUserService users, IApplicationDbContext context)
    {
        _filterFactory = filterFactory;
        _users = users;
        _context = context;
    }

    public async Task<IEnumerable<SearchAccountResponse>> Handle(SearchAccountQuery request, CancellationToken cancellationToken)
    {
        // Filtering
        var userFilter = _filterFactory.CreateCaseInsensitiveContainsFilter<IUser>(user => user.Email!, request.Email);

        var accountFilter = IFilter<Account>.Combine(
            _filterFactory.CreateCaseInsensitiveContainsFilter<Account>(account => account.FirstName, request.FirstName),
            _filterFactory.CreateCaseInsensitiveContainsFilter<Account>(account => account.LastName, request.LastName));

        var accounts =
            from user in _users.Users.Where(userFilter)
            from account in _context.Accounts.Where(accountFilter)
            select new { User = user, Account = account };

        // Pagination
        accounts = accounts
            .OrderBy(x => x.User.Id)
            .Skip(request.From)
            .Take(request.Size);

        return await accounts
            .Select(x => new SearchAccountResponse(
                x.User.Id,
                x.Account.FirstName,
                x.Account.LastName,
                x.User.Email!))
            .ToListAsync(cancellationToken);
    }
}