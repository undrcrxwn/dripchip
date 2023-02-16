using DripChip.Application.Abstractions.Identity;
using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using DripChip.Application.Extensions;
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
        private readonly IApplicationDbContext _context;
        private readonly IUserService _users;

        public Handler(IApplicationDbContext context, IUserService users)
        {
            _context = context;
            _users = users;
        }

        public async ValueTask<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var user =
                await _users.FindByIdAsync(request.Id)
                ?? throw new NotFoundException();
        
            var account =
                await _context.Accounts.FindAsync(request.Id)
                ?? throw new NotFoundException();

            return new Response(user.Id, account.FirstName, account.LastName, user.Email!);
        }
    }

    public sealed record Response(int Id, string FirstName, string LastName, string Email);
}