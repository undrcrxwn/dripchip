using DripChip.Application.Abstractions;
using DripChip.Application.Abstractions.Identity;
using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using DripChip.Application.Extensions;
using FluentValidation;
using Mediator;
using Microsoft.EntityFrameworkCore;
using ValidationException = DripChip.Application.Exceptions.ValidationException;

namespace DripChip.Application.Features.Accounts.Commands;

public static class Delete
{
    public sealed record Command(int Id) : IRequest;

    public sealed class Validator : AbstractValidator<Command>
    {
        public Validator() => RuleFor(x => x.Id).AccountId();
    }

    internal sealed class Handler : IRequestHandler<Command>
    {
        private readonly IApplicationDbContext _context;
        private readonly IUserService _users;
        private readonly ICurrentUserProvider _issuer;

        public Handler(IApplicationDbContext context, IUserService users, ICurrentUserProvider issuer)
        {
            _context = context;
            _users = users;
            _issuer = issuer;
        }

        public async ValueTask<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            if (request.Id != _issuer.AccountId)
                throw new ForbiddenException();

            var account =
                await _context.Accounts
                    .Include(account => account.ChippedAnimals)
                    .FirstOrDefaultAsync(account => account.Id == request.Id, cancellationToken)
                ?? throw new NotFoundException();

            if (account.ChippedAnimals.Any())
                throw new ValidationException(nameof(request.Id), "The specified account owns one or more chipped animals.");

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync(cancellationToken);
            await _users.DeleteAsync(request.Id);

            return Unit.Value;
        }
    }
}