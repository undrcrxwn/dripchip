using DripChip.Application.Abstractions;
using DripChip.Application.Abstractions.Identity;
using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Abstractions.Specifications;
using DripChip.Application.Exceptions;
using DripChip.Application.Extensions;
using DripChip.Domain.Constants;
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
        private readonly ICurrentUserProvider _issuer;
        private readonly IApplicationDbContext _context;
        private readonly IUserRepository _users;
        private readonly ISpecificationFactory _specifications;

        public Handler(ICurrentUserProvider issuer, IApplicationDbContext context, IUserRepository users, ISpecificationFactory specifications)
        {
            _issuer = issuer;
            _context = context;
            _users = users;
            _specifications = specifications;
        }

        public async ValueTask<IEnumerable<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            var issuer = await _users.FindByIdAsync(_issuer.AccountId!.Value);
            if (issuer!.Role != Roles.Admin)
                throw new ForbiddenException();

            // Filtering
            var emailFilter = _specifications.CaseInsensitiveContains(request.Email);
            var firstNameFilter = _specifications.CaseInsensitiveContains(request.FirstName);
            var lastNameFilter = _specifications.CaseInsensitiveContains(request.LastName);

            var userAccounts =
                from user in _users.Users.Where(x => x.Email!, emailFilter)
                join account in _context.Accounts
                        .Where(x => x.FirstName, firstNameFilter)
                        .Where(x => x.LastName, lastNameFilter)
                    on user.Id equals account.Id
                select new { User = user, Account = account };

            // Pagination
            userAccounts = userAccounts
                .OrderBy(x => x.User.Id)
                .Skip(request.From)
                .Take(request.Size);

            return await userAccounts
                .Select(userAccount => new Response(
                    userAccount.User.Id,
                    userAccount.Account.FirstName,
                    userAccount.Account.LastName,
                    userAccount.User.Email!,
                    userAccount.User.Role))
                .ToListAsync(cancellationToken);
        }
    }

    public sealed record Response(int Id, string FirstName, string LastName, string Email, string Role);
}