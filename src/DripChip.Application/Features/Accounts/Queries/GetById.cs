using DripChip.Application.Abstractions.Identity;
using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using DripChip.Application.Extensions;
using FluentValidation;
using Mediator;
using Microsoft.EntityFrameworkCore;

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
        private readonly IApplicationDbContext _context;
        private readonly IUserRepository _users;

        public Handler(IApplicationDbContext context, IUserRepository users)
        {
            _context = context;
            _users = users;
        }

        public async ValueTask<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var query =
                from account in _context.Accounts
                where account.Id == request.Id
                join user in _users.Users on request.Id equals user.Id
                select new Response(
                    user.Id,
                    account.FirstName,
                    account.LastName,
                    user.Email);

            return await query.FirstOrDefaultAsync(cancellationToken)
                   ?? throw new NotFoundException();
        }
    }

    public sealed record Response(int Id, string FirstName, string LastName, string Email);
}