using DripChip.Application.Abstractions;
using DripChip.Application.Abstractions.Identity;
using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using DripChip.Application.Extensions;
using DripChip.Domain.Constants;
using FluentValidation;
using Mapster;
using Mediator;

namespace DripChip.Application.Features.Accounts.Queries;

public static class GetById
{
    public sealed record Query(int Id) : IRequest<Response>;

    public sealed class Validator : AbstractValidator<Query>
    {
        public Validator() => RuleFor(x => x.Id).AccountId();
    }

    internal sealed class Handler : IRequestHandler<Query, Response>
    {
        private readonly ICurrentUserProvider _issuer;
        private readonly IApplicationDbContext _context;

        public Handler(ICurrentUserProvider issuer, IApplicationDbContext context)
        {
            _issuer = issuer;
            _context = context;
        }

        public async ValueTask<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var issuer = await _issuer.GetAccountAsync();
            if (issuer?.Id != request.Id && issuer?.Role != Roles.Admin)
                throw new ForbiddenException();

            var account =
                await _context.Accounts.FindAsync(request.Id)
                ?? throw new NotFoundException();

            return account.Adapt<Response>();
        }
    }

    public sealed record Response(int Id, string FirstName, string LastName, string Email, string Role);
}