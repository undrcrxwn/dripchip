using DripChip.Application.Abstractions.Identity;
using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using DripChip.Domain.Entities;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DripChip.Application.Features.Accounts.Commands.Create;

public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, CreateAccountResponse>
{
    private readonly IUserService _users;
    private readonly IApplicationDbContext _context;

    public CreateAccountCommandHandler(IUserService users, IApplicationDbContext context)
    {
        _users = users;
        _context = context;
    }
    
    public async Task<CreateAccountResponse> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var sameExists = await _users.Users.AnyAsync(user =>
            user.Email == request.Email, cancellationToken);
        
        if (sameExists)
            throw new AlreadyExistsException("User with the specified email already exists.");

        // User creation
        var errorDescriptions = await _users.CreateAsync(request.Email, request.Password);
        if (errorDescriptions is not null)
            throw new ValidationException(errorDescriptions.ToArray(), nameof(request.Password));
        
        var user =
            await _users.FindByEmailAsync(request.Email)
            ?? throw new InvalidOperationException();
        
        // Account creation
        var account = request.Adapt<Account>();
        account.Id = user.Id;
        await _context.Accounts.AddAsync(account, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
        return new CreateAccountResponse(user.Id, account.FirstName, account.LastName, user.Email!);
    }
}