using DripChip.Application.Exceptions;
using DripChip.Application.Features.Accounts.Commands.Update;
using DripChip.Infrastructure.Identity.Extensions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace DripChip.Infrastructure.Identity.CQRS.Commands;

public class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand, UpdateAccountResponse>
{
    private readonly UserManager<Account> _userManager;
    
    public UpdateAccountCommandHandler(UserManager<Account> userManager) =>
        _userManager = userManager;

    
    public async Task<UpdateAccountResponse> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        var account =
            await _userManager.FindByIdAsync(request.AccountId.ToString())
            ?? throw new NotFoundException();

        account.FirstName = request.FirstName;
        account.LastName = request.LastName;
        account.Email = request.Email;
        
        // Implicitly calls UpdateUserAsync(account)
        await _userManager.SetPasswordAsync(account, request.Password);

        return new UpdateAccountResponse(account.Id, account.FirstName, account.LastName, account.Email);
    }
}