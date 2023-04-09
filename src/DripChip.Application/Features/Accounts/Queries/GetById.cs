using DripChip.Application.Abstractions;
using DripChip.Application.Abstractions.Identity;
using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using DripChip.Application.Extensions;
using DripChip.Domain.Constants;
using FluentValidation;
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
        private readonly IUserRepository _users;

        public Handler(ICurrentUserProvider issuer, IApplicationDbContext context, IUserRepository users)
        {
            _issuer = issuer;
            _context = context;
            _users = users;
        }

        public async ValueTask<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var issuer = await _users.FindByIdAsync(_issuer.AccountId!.Value);
            if (issuer?.Id != request.Id && issuer?.Role != Roles.Admin)
                throw new ForbiddenException();

            var user =
                await _users.FindByIdAsync(request.Id)
                ?? throw new NotFoundException();

            var account =
                await _context.Accounts.FindAsync(request.Id)
                ?? throw new NotFoundException();

            return new Response(user.Id, account.FirstName, account.LastName, user.Email!, user.Role);
        }
    }

    public sealed record Response(int Id, string FirstName, string LastName, string Email, string Role);
}