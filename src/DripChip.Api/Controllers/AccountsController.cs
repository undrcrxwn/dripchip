using DripChip.Api.Attributes;
using DripChip.Application.Features.Accounts.Commands.Delete;
using DripChip.Application.Features.Accounts.Commands.Register;
using DripChip.Application.Features.Accounts.Commands.Update;
using DripChip.Application.Features.Accounts.Queries.GetById;
using DripChip.Application.Features.Accounts.Queries.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.Api.Controllers;

public class AccountsController : ApiControllerBase
{
    [HttpPost, ApiRoute("~/registration")]
    public async Task<IActionResult> Register([FromBody] RegisterAccountCommand command)
    {
        var response = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { AccountId = response.Id }, response);
    }

    [HttpGet("{accountId}")]
    public async Task<GetAccountByIdResponse> GetById([FromRoute] int accountId) =>
        await Mediator.Send(new GetAccountByIdQuery(accountId));

    [HttpGet("[action]")]
    public async Task<IEnumerable<SearchAccountResponse>> Search([FromQuery] SearchAccountQuery query) =>
        await Mediator.Send(query);

    [HttpPut("{accountId}"), Authorize]
    public async Task<UpdateAccountResponse> Update([FromRoute] int accountId, [FromBody] UpdateAccountCommand command) =>
        await Mediator.Send(command with { Id = accountId });
    
    [HttpDelete("{accountId}"), Authorize]
    public async Task Delete([FromRoute] int accountId) =>
        await Mediator.Send(new DeleteAccountCommand(accountId));
}