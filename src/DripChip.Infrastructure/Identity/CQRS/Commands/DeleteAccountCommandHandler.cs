using DripChip.Application.Abstractions;
using DripChip.Application.Exceptions;
using DripChip.Application.Features.Accounts.Commands.Delete;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace DripChip.Infrastructure.Identity.CQRS.Commands;

public class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand>
{
    private readonly ICurrentUserProvider _issuer;
    private readonly UserManager<Account> _userManager;

    public DeleteAccountCommandHandler(ICurrentUserProvider issuer, UserManager<Account> userManager)
    {
        _issuer = issuer;
        _userManager = userManager;
    }

    public async Task<Unit> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        if (request.AccountId != _issuer.AccountId)
            throw new ForbiddenException();

        var account = await _userManager.FindByIdAsync(request.AccountId.ToString());
        await _userManager.DeleteAsync(account!);
        
        return Unit.Value;
    }
}