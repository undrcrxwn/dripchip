using DripChip.Application.Abstractions.Identity;
using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using MediatR;

namespace DripChip.Application.Features.Accounts.Queries.GetById;

public class GetAccountByIdQueryHandler : IRequestHandler<GetAccountByIdQuery, GetAccountByIdResponse>
{
    private readonly IUserService _users;
    private readonly IApplicationDbContext _context;

    public GetAccountByIdQueryHandler(IUserService users, IApplicationDbContext context)
    {
        _users = users;
        _context = context;
    }

    public async Task<GetAccountByIdResponse> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
    {
        var user =
            await _users.FindByIdAsync(request.AccountId)
            ?? throw new NotFoundException();
        
        var account =
            await _context.Accounts.FindAsync(request.AccountId)
            ?? throw new NotFoundException();

        return new GetAccountByIdResponse(user.Id, account.FirstName, account.LastName, user.Email);
    }
}