using DripChip.Application.Features.Areas.Commands;
using DripChip.Application.Features.Areas.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.Api.Controllers;

[Authorize]
public sealed class AreasController : ApiControllerBase
{
    [HttpGet("{areaId}")]
    public async Task<GetById.Response> GetById([FromRoute] long areaId) =>
        await Mediator.Send(new GetById.Query(areaId));

    [HttpPost]
    public async Task<ActionResult<Create.Response>> Create([FromBody] Create.Command command)
    {
        var response = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { AreaId = response.Id }, response);
    }

    [HttpPut("{areaId}")]
    public async Task<Update.Response> Update([FromRoute] long areaId, [FromBody] Update.Command command) =>
        await Mediator.Send(command with { Id = areaId });

    [HttpDelete("{areaId}")]
    public async Task Delete([FromRoute] long areaId) =>
        await Mediator.Send(new Delete.Command(areaId));
}