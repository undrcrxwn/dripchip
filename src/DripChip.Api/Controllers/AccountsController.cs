using DripChip.Api.Routing;
using DripChip.Application.Features.Accounts.Commands;
using DripChip.Application.Features.Accounts.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace DripChip.Api.Controllers;

public sealed class AccountsController : ApiControllerBase
{
    [ProducesResponseType(Status201Created)]
    [ProducesResponseType(Status400BadRequest)]
    [ProducesResponseType(Status403Forbidden)]
    [ProducesResponseType(Status409Conflict)]
    [HttpPost, ApiRoute("~/registration")]
    public async Task<ActionResult<Register.Response>> Register([FromBody] Register.Command command)
    {
        var response = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { AccountId = response.Id }, response);
    }

    [ProducesResponseType(Status200OK)]
    [ProducesResponseType(Status400BadRequest)]
    [ProducesResponseType(Status401Unauthorized)]
    [ProducesResponseType(Status404NotFound)]
    [HttpGet("{accountId}")]
    public async Task<GetById.Response> GetById([FromRoute] int accountId) =>
        await Mediator.Send(new GetById.Query(accountId));

    [ProducesResponseType(Status200OK)]
    [ProducesResponseType(Status400BadRequest)]
    [ProducesResponseType(Status401Unauthorized)]
    [HttpGet("[action]")]
    public async Task<IEnumerable<Search.Response>> Search([FromQuery] Search.Query query) =>
        await Mediator.Send(query);

    [ProducesResponseType(Status200OK)]
    [ProducesResponseType(Status400BadRequest)]
    [ProducesResponseType(Status401Unauthorized)]
    [ProducesResponseType(Status403Forbidden)]
    [ProducesResponseType(Status404NotFound)]
    [HttpPut("{accountId}"), Authorize]
    public async Task<Update.Response> Update([FromRoute] int accountId, [FromBody] Update.Command command) =>
        await Mediator.Send(command with { Id = accountId });

    [ProducesResponseType(Status200OK)]
    [ProducesResponseType(Status400BadRequest)]
    [ProducesResponseType(Status401Unauthorized)]
    [ProducesResponseType(Status403Forbidden)]
    [HttpDelete("{accountId}"), Authorize]
    public async Task Delete([FromRoute] int accountId)
    {
        await Mediator.Send(new Delete.Command(accountId));
        foreach (var cookie in Request.Cookies.Keys)
            Response.Cookies.Delete(cookie);
    }
}