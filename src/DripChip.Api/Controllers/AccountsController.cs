using DripChip.Application.Features.Accounts.Commands.Register;
using DripChip.Application.Features.Accounts.Commands.Update;
using DripChip.Application.Features.Accounts.Queries.GetById;
using DripChip.Application.Features.Accounts.Queries.Search;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.Api.Controllers;

[Route("[controller]")]
public class AccountsController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public AccountsController(IMediator mediator) =>
        _mediator = mediator;

    [HttpPost("~/registration")]
    public async Task<RegisterAccountResponse> Register([FromBody] RegisterAccountCommand command) =>
        await _mediator.Send(command);

    [HttpGet("{accountId}")]
    public async Task<GetAccountByIdResponse> GetById([FromRoute] int accountId) =>
        await _mediator.Send(new GetAccountByIdQuery(accountId));

    [HttpGet("[action]")]
    public async Task<IEnumerable<SearchAccountResponse>> Search([FromQuery] SearchAccountQuery query) =>
        await _mediator.Send(query);

    [HttpPut("{accountId}")]
    public async Task<UpdateAccountResponse> Update([FromRoute] int accountId, [FromQuery] UpdateAccountCommand command) =>
        await _mediator.Send(command with { AccountId = accountId });
}