using DripChip.Application.Features.LocationPoints.Commands;
using DripChip.Application.Features.LocationPoints.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace DripChip.Api.Controllers;

public sealed class LocationsController : ApiControllerBase
{
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType(Status400BadRequest)]
    [ProducesResponseType(Status401Unauthorized)]
    [ProducesResponseType(Status404NotFound)]
    [HttpGet("{pointId}")]
    public async Task<GetById.Response> GetById([FromRoute] long pointId) =>
        await Mediator.Send(new GetById.Query(pointId));

    [ProducesResponseType(Status201Created)]
    [ProducesResponseType(Status400BadRequest)]
    [ProducesResponseType(Status401Unauthorized)]
    [ProducesResponseType(Status409Conflict)]
    [HttpPost, Authorize]
    public async Task<ActionResult<Create.Response>> Create([FromBody] Create.Command command)
    {
        var response = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { PointId = response.Id }, response);
    }

    [ProducesResponseType(Status200OK)]
    [ProducesResponseType(Status400BadRequest)]
    [ProducesResponseType(Status401Unauthorized)]
    [ProducesResponseType(Status404NotFound)]
    [ProducesResponseType(Status409Conflict)]
    [HttpPut("{pointId}"), Authorize]
    public async Task<Update.Response> Update([FromRoute] long pointId, [FromBody] Update.Command command) =>
        await Mediator.Send(command with { Id = pointId });

    [ProducesResponseType(Status200OK)]
    [ProducesResponseType(Status400BadRequest)]
    [ProducesResponseType(Status401Unauthorized)]
    [ProducesResponseType(Status404NotFound)]
    [HttpDelete("{pointId}"), Authorize]
    public async Task Delete([FromRoute] long pointId) =>
        await Mediator.Send(new Delete.Command(pointId));
}