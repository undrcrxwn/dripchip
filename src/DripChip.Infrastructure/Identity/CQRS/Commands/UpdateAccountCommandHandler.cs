using DripChip.Application.Abstractions;
using DripChip.Application.Exceptions;
using DripChip.Application.Features.Accounts.Commands.Update;
using DripChip.Infrastructure.Identity.Extensions;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace DripChip.Infrastructure.Identity.CQRS.Commands;

public class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand, UpdateAccountResponse>
{
    private readonly ICurrentUserProvider _issuer;
    private readonly UserManager<Account> _userManager;

    public UpdateAccountCommandHandler(ICurrentUserProvider issuer, UserManager<Account> userManager)
    {
        _issuer = issuer;
        _userManager = userManager;
    }

    public async Task<UpdateAccountResponse> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        if (request.AccountId != _issuer.AccountId)
            throw new ForbiddenException();
        
        var account =
            await _userManager.FindByIdAsync(request.AccountId.ToString())
            ?? throw new NotFoundException();

        account.FirstName = request.FirstName;
        account.LastName = request.LastName;
        account.Email = request.Email;
        
        // Implicitly calls UpdateUserAsync(account)
        await _userManager.SetPasswordAsync(account, request.Password);

        return account.Adapt<UpdateAccountResponse>();
    }
}