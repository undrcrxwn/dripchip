using DripChip.Application.Abstractions;
using DripChip.Application.Abstractions.Identity;
using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using MediatR;

namespace DripChip.Application.Features.Accounts.Commands.Update;

public class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand, UpdateAccountResponse>
{
    private readonly ICurrentUserProvider _issuer;
    private readonly IUserService _users;
    private readonly IApplicationDbContext _context;

    public UpdateAccountCommandHandler(ICurrentUserProvider issuer, IUserService users, IApplicationDbContext context)
    {
        _issuer = issuer;
        _users = users;
        _context = context;
    }

    public async Task<UpdateAccountResponse> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        if (request.Id != _issuer.AccountId)
            throw new ForbiddenException();
        
        var user =
            await _users.FindByIdAsync(request.Id)
            ?? throw new NotFoundException();
        
        var account =
            await _context.Accounts.FindAsync(request.Id)
            ?? throw new NotFoundException();

        account.FirstName = request.FirstName;
        account.LastName = request.LastName;
        
        await _users.SetPasswordAsync(user, request.Password);
        await _context.SaveChangesAsync(cancellationToken);
        
        return new UpdateAccountResponse(
            user.Id,
            account.FirstName,
            account.LastName,
            user.Email!);
    }
}