using DripChip.Application.Abstractions;
using DripChip.Application.Abstractions.Identity;
using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Abstractions.Specifications;
using DripChip.Application.Exceptions;
using DripChip.Application.Extensions;
using DripChip.Domain.Constants;
using FluentValidation;
using Mapster;
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
        private readonly ISpecificationFactory _specifications;

        public Handler(ICurrentUserProvider issuer, IApplicationDbContext context, ISpecificationFactory specifications)
        {
            _issuer = issuer;
            _context = context;
            _specifications = specifications;
        }

        public async ValueTask<IEnumerable<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            var issuer = await _issuer.GetAccountAsync();
            if (issuer!.Role != Roles.Admin)
                throw new ForbiddenException();

            // Filtering
            var emailFilter = _specifications.CaseInsensitiveContains(request.Email);
            var firstNameFilter = _specifications.CaseInsensitiveContains(request.FirstName);
            var lastNameFilter = _specifications.CaseInsensitiveContains(request.LastName);

            var accounts = _context.Accounts
                .Where(account => account.Email!, emailFilter)
                .Where(account => account.FirstName, firstNameFilter)
                .Where(account => account.LastName, lastNameFilter)
                // Pagination
                .OrderBy(account => account.Id)
                .Skip(request.From)
                .Take(request.Size);

            return await accounts.ProjectToType<Response>().ToListAsync(cancellationToken);
        }
    }

    public sealed record Response(int Id, string FirstName, string LastName, string Email, string Role);
}