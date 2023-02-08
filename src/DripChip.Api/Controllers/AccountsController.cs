using DripChip.Api.Attributes;
using DripChip.Application.Features.Accounts.Commands.Delete;
using DripChip.Application.Features.Accounts.Commands.Register;
using DripChip.Application.Features.Accounts.Commands.Update;
using DripChip.Application.Features.Accounts.Queries.GetById;
using DripChip.Application.Features.Accounts.Queries.Search;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.Api.Controllers;

public class AccountsController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public AccountsController(IMediator mediator) =>
        _mediator = mediator;

    [HttpPost, ApiRoute("~/registration")]
    public async Task<IActionResult> Register([FromBody] RegisterAccountCommand command)
    {
        var response = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { AccountId = response.Id }, response);
    }

    [HttpGet("{accountId}")]
    public async Task<GetAccountByIdResponse> GetById([FromRoute] int accountId) =>
        await _mediator.Send(new GetAccountByIdQuery(accountId));

    [HttpGet("[action]")]
    public async Task<IEnumerable<SearchAccountResponse>> Search([FromQuery] SearchAccountQuery query) =>
        await _mediator.Send(query);

    [HttpPut("{accountId}"), Authorize]
    public async Task<UpdateAccountResponse> Update([FromRoute] int accountId, [FromBody] UpdateAccountCommand command) =>
        await _mediator.Send(command with { AccountId = accountId });
    
    [HttpDelete("{accountId}"), Authorize]
    public async Task Delete([FromRoute] int accountId) =>
        await _mediator.Send(new DeleteAccountCommand(accountId));
}