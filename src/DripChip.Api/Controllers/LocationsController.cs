using DripChip.Application.Features.LocationPoints.Commands.Create;
using DripChip.Application.Features.LocationPoints.Commands.Delete;
using DripChip.Application.Features.LocationPoints.Commands.Update;
using DripChip.Application.Features.LocationPoints.Queries.GetById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.Api.Controllers;

public class LocationsController : ApiControllerBase
{
    [HttpGet("{pointId}")]
    public async Task<GetLocationPointByIdResponse> GetById([FromRoute] int pointId) =>
        await Mediator.Send(new GetLocationPointByIdQuery(pointId));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateLocationPointCommand command)
    {
        var response = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { PointId = response.Id }, response);
    }

    [HttpPut("{pointId}"), Authorize]
    public async Task<UpdateLocationPointResponse> Update([FromRoute] long pointId, [FromBody] UpdateLocationPointCommand command) =>
        await Mediator.Send(command with { Id = pointId });
    
    [HttpDelete("{pointId}"), Authorize]
    public async Task Delete([FromRoute] long pointId) =>
        await Mediator.Send(new DeleteLocationPointCommand(pointId));
}