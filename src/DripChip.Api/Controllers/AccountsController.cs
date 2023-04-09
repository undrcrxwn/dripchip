using DripChip.Api.Routing;
using DripChip.Application.Features.Accounts.Commands;
using DripChip.Application.Features.Accounts.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace DripChip.Api.Controllers;

[Authorize]
public sealed class AccountsController : ApiControllerBase
{
    [HttpPost, ApiRoute("~/registration"), AllowAnonymous]
    public async Task<ActionResult<Register.Response>> Register([FromBody] Register.Command command)
    {
        var response = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { AccountId = response.Id }, response);
    }

    [HttpGet("{accountId}")]
    public async Task<GetById.Response> GetById([FromRoute] int accountId) =>
        await Mediator.Send(new GetById.Query(accountId));

    [HttpGet("[action]")]
    public async Task<IEnumerable<Search.Response>> Search([FromQuery] Search.Query query) =>
        await Mediator.Send(query);

    [HttpPost("[action]")]
    public async Task<Create.Response> Create([FromBody] Create.Command command) =>
        await Mediator.Send(command);

    [HttpPut("{accountId}")]
    public async Task<Update.Response> Update([FromRoute] int accountId, [FromBody] Update.Command command) =>
        await Mediator.Send(command with { Id = accountId });

    [HttpDelete("{accountId}")]
    public async Task Delete([FromRoute] int accountId)
    {
        await Mediator.Send(new Delete.Command(accountId));
        foreach (var cookie in Request.Cookies.Keys)
            Response.Cookies.Delete(cookie);
    }
}