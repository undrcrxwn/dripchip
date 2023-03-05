using DripChip.Api.Routing;
using DripChip.Application.Features.AnimalTypes.Commands;
using DripChip.Application.Features.AnimalTypes.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.Api.Controllers;

[ApiRoute("animals/types")]
public sealed class AnimalTypesController : ApiControllerBase
{
    [HttpGet("{typeId}")]
    public async Task<GetById.Response> GetById([FromRoute] long typeId) =>
        await Mediator.Send(new GetById.Query(typeId));

    [HttpPost, Authorize]
    public async Task<ActionResult<Create.Response>> Create([FromBody] Create.Command command)
    {
        var response = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { TypeId = response.Id }, response);
    }

    [HttpPut("{typeId}"), Authorize]
    public async Task<Update.Response> Update([FromRoute] long typeId, [FromBody] Update.Command command) =>
        await Mediator.Send(command with { Id = typeId });
    
    [HttpDelete("{typeId}"), Authorize]
    public async Task Delete([FromRoute] long typeId) =>
        await Mediator.Send(new Delete.Command(typeId));
}