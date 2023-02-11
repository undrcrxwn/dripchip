using DripChip.Application.Abstractions;
using DripChip.Application.Abstractions.Identity;
using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using MediatR;

namespace DripChip.Application.Features.Accounts.Commands.Delete;

public class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand>
{
    private readonly ICurrentUserProvider _issuer;
    private readonly IUserService _users;
    private readonly IApplicationDbContext _context;

    public DeleteAccountCommandHandler(ICurrentUserProvider issuer, IUserService users, IApplicationDbContext context)
    {
        _issuer = issuer;
        _users = users;
        _context = context;
    }

    public async Task<Unit> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        if (request.Id != _issuer.AccountId)
            throw new ForbiddenException();

        var account =
            await _context.Accounts.FindAsync(request.Id)
            ?? throw new NotFoundException();

        _context.Accounts.Remove(account);
        await _users.DeleteAsync(request.Id);

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}