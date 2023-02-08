using DripChip.Application.Exceptions;
using DripChip.Application.Features.Accounts.Commands.Register;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace DripChip.Infrastructure.Identity.CQRS.Commands;

public class RegisterAccountCommandHandler : IRequestHandler<RegisterAccountCommand, RegisterAccountResponse>
{
    private readonly UserManager<Account> _userManager;
    
    public RegisterAccountCommandHandler(UserManager<Account> userManager) =>
        _userManager = userManager;

    public async Task<RegisterAccountResponse> Handle(RegisterAccountCommand request, CancellationToken cancellationToken)
    {
        // Email uniqueness validation
        var account = await _userManager.FindByEmailAsync(request.Email);
        if (account is not null)
            throw new AlreadyExistsException("Account with the specified email already exists.");

        account = new Account
        {
            UserName = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email
        };

        // User creation
        var result = await _userManager.CreateAsync(account, request.Password);
        if (!result.Succeeded)
            throw new ValidationException(result.Errors, nameof(request.Password));

        return account.Adapt<RegisterAccountResponse>();
    }
}