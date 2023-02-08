using DripChip.Application.Exceptions;
using DripChip.Application.Features.Accounts.Queries.GetById;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace DripChip.Infrastructure.Identity.CQRS.Queries;

public class GetAccountByIdQueryHandler : IRequestHandler<GetAccountByIdQuery, GetAccountByIdResponse>
{
    private readonly UserManager<Account> _userManager;
    
    public GetAccountByIdQueryHandler(UserManager<Account> userManager) =>
        _userManager = userManager;

    public async Task<GetAccountByIdResponse> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
    {
        var account =
            await _userManager.FindByIdAsync(request.AccountId.ToString())
            ?? throw new NotFoundException();

        return new GetAccountByIdResponse(
            account.Id, account.FirstName, account.LastName, account.Email!);
    }
}