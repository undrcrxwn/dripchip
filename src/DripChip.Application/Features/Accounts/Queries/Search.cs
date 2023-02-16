using DripChip.Application.Abstractions.Filtering;
using DripChip.Application.Abstractions.Identity;
using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Extensions;
using DripChip.Domain.Entities;
using FluentValidation;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace DripChip.Application.Features.Accounts.Queries;

public static class Search
{
    public sealed record Query(
        string? FirstName = default,
        string? LastName = default,
        string? Email = default,
        int From = 0,
        int Size = 10) : IRequest<IEnumerable<Response>>;

    public sealed class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.From).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Size).GreaterThan(0);
        }
    }

    internal sealed class Handler : IRequestHandler<Query, IEnumerable<Response>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IUserService _users;
        private readonly IFilterFactory _filterFactory;

        public Handler(IApplicationDbContext context, IUserService users, IFilterFactory filterFactory)
        {
            _context = context;
            _users = users;
            _filterFactory = filterFactory;
        }

        public async ValueTask<IEnumerable<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            // Filtering
            var userFilter = _filterFactory.CreateCaseInsensitiveContainsFilter<IUser>(user => user.Email!, request.Email);

            var accountFilter = IFilter<Account>.Combine(
                _filterFactory.CreateCaseInsensitiveContainsFilter<Account>(account => account.FirstName, request.FirstName),
                _filterFactory.CreateCaseInsensitiveContainsFilter<Account>(account => account.LastName, request.LastName));

            var accounts =
                from user in _users.Users.Where(userFilter)
                join account in _context.Accounts.Where(accountFilter) on user.Id equals account.Id
                select new { User = user, Account = account };

            // Pagination
            accounts = accounts
                .OrderBy(x => x.User.Id)
                .Skip(request.From)
                .Take(request.Size);

            return await accounts
                .Select(x => new Response(
                    x.User.Id,
                    x.Account.FirstName,
                    x.Account.LastName,
                    x.User.Email!))
                .ToListAsync(cancellationToken);
        }
    }

    public sealed record Response(int Id, string FirstName, string LastName, string Email);
}