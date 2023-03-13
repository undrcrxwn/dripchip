using DripChip.Api.Routing;
using DripChip.Application.Features.AnimalTypes.Commands;
using DripChip.Application.Features.AnimalTypes.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace DripChip.Api.Controllers;

[ApiRoute("animals/types")]
public sealed class AnimalTypesController : ApiControllerBase
{
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType(Status400BadRequest)]
    [ProducesResponseType(Status401Unauthorized)]
    [ProducesResponseType(Status404NotFound)]
    [HttpGet("{typeId}")]
    public async Task<GetById.Response> GetById([FromRoute] long typeId) =>
        await Mediator.Send(new GetById.Query(typeId));

    [ProducesResponseType(Status201Created)]
    [ProducesResponseType(Status400BadRequest)]
    [ProducesResponseType(Status401Unauthorized)]
    [ProducesResponseType(Status409Conflict)]
    [HttpPost, Authorize]
    public async Task<ActionResult<Create.Response>> Create([FromBody] Create.Command command)
    {
        var response = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { TypeId = response.Id }, response);
    }

    [ProducesResponseType(Status200OK)]
    [ProducesResponseType(Status400BadRequest)]
    [ProducesResponseType(Status401Unauthorized)]
    [ProducesResponseType(Status404NotFound)]
    [ProducesResponseType(Status409Conflict)]
    [HttpPut("{typeId}"), Authorize]
    public async Task<Update.Response> Update([FromRoute] long typeId, [FromBody] Update.Command command) =>
        await Mediator.Send(command with { Id = typeId });
    
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType(Status400BadRequest)]
    [ProducesResponseType(Status401Unauthorized)]
    [ProducesResponseType(Status404NotFound)]
    [HttpDelete("{typeId}"), Authorize]
    public async Task Delete([FromRoute] long typeId) =>
        await Mediator.Send(new Delete.Command(typeId));
}