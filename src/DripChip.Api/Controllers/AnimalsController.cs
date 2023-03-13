using DripChip.Application.Features.Animals.Commands;
using DripChip.Application.Features.Animals.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace DripChip.Api.Controllers;

public sealed class AnimalsController : ApiControllerBase
{
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType(Status400BadRequest)]
    [ProducesResponseType(Status401Unauthorized)]
    [ProducesResponseType(Status404NotFound)]
    [HttpGet("{animalId}")]
    public async Task<GetById.Response> GetById([FromRoute] long animalId) =>
        await Mediator.Send(new GetById.Query(animalId));

    [ProducesResponseType(Status200OK)]
    [ProducesResponseType(Status400BadRequest)]
    [ProducesResponseType(Status401Unauthorized)]
    [HttpGet("[action]")]
    public async Task<IEnumerable<Search.Response>> Search([FromQuery] Search.Query query) =>
        await Mediator.Send(query);

    [ProducesResponseType(Status201Created)]
    [ProducesResponseType(Status400BadRequest)]
    [ProducesResponseType(Status401Unauthorized)]
    [ProducesResponseType(Status404NotFound)]
    [ProducesResponseType(Status409Conflict)]
    [HttpPost, Authorize]
    public async Task<ActionResult<Create.Response>> Create([FromBody] Create.Command command)
    {
        var response = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { AnimalId = response.Id }, response);
    }

    [ProducesResponseType(Status200OK)]
    [ProducesResponseType(Status400BadRequest)]
    [ProducesResponseType(Status401Unauthorized)]
    [ProducesResponseType(Status404NotFound)]
    [HttpPut("{animalId}"), Authorize]
    public async Task<Update.Response> Update([FromRoute] long animalId, [FromBody] Update.Command command) =>
        await Mediator.Send(command with { Id = animalId });

    [ProducesResponseType(Status200OK)]
    [ProducesResponseType(Status400BadRequest)]
    [ProducesResponseType(Status401Unauthorized)]
    [ProducesResponseType(Status404NotFound)]
    [HttpDelete("{animalId}"), Authorize]
    public async Task Delete([FromRoute] long animalId) =>
        await Mediator.Send(new Delete.Command(animalId));

    #region AnimalTypes

    [ProducesResponseType(Status201Created)]
    [ProducesResponseType(Status400BadRequest)]
    [ProducesResponseType(Status401Unauthorized)]
    [ProducesResponseType(Status404NotFound)]
    [ProducesResponseType(Status409Conflict)]
    [HttpPost("{animalId}/types/{typeId}"), Authorize]
    public async Task<ActionResult<AddType.Response>> AddType([FromRoute] long animalId, [FromRoute] long typeId)
    {
        var response = await Mediator.Send(new AddType.Command(animalId, typeId));
        return CreatedAtAction(nameof(GetById), new { AnimalId = response.Id }, response);
    }

    [ProducesResponseType(Status200OK)]
    [ProducesResponseType(Status400BadRequest)]
    [ProducesResponseType(Status401Unauthorized)]
    [ProducesResponseType(Status404NotFound)]
    [ProducesResponseType(Status409Conflict)]
    [HttpPut("{animalId}/types"), Authorize]
    public async Task<ReplaceType.Response> ReplaceType([FromRoute] long animalId, [FromBody] ReplaceType.Command command) =>
        await Mediator.Send(command with { Id = animalId });

    [ProducesResponseType(Status200OK)]
    [ProducesResponseType(Status400BadRequest)]
    [ProducesResponseType(Status401Unauthorized)]
    [ProducesResponseType(Status404NotFound)]
    [HttpDelete("{animalId}/types/{typeId}"), Authorize]
    public async Task<RemoveType.Response> RemoveType([FromRoute] long animalId, [FromRoute] long typeId) =>
        await Mediator.Send(new RemoveType.Command(animalId, typeId));

    #endregion

    #region Locations

    [ProducesResponseType(Status200OK)]
    [ProducesResponseType(Status400BadRequest)]
    [ProducesResponseType(Status401Unauthorized)]
    [ProducesResponseType(Status404NotFound)]
    [HttpGet("{animalId}/locations")]
    public async Task<IEnumerable<SearchVisits.Response>> SearchVisits(
        [FromRoute] long animalId, [FromQuery] SearchVisits.Query query) =>
        await Mediator.Send(query with { Id = animalId });

    [ProducesResponseType(Status201Created)]
    [ProducesResponseType(Status400BadRequest)]
    [ProducesResponseType(Status401Unauthorized)]
    [ProducesResponseType(Status404NotFound)]
    [HttpPost("{animalId}/locations/{pointId}"), Authorize]
    public async Task<IActionResult> AddVisit([FromRoute] long animalId, [FromRoute] long pointId)
    {
        var response = await Mediator.Send(new AddVisit.Command(animalId, pointId));
        return CreatedAtAction(nameof(GetById), new { AnimalId = response.Id }, response);
    }

    [ProducesResponseType(Status200OK)]
    [ProducesResponseType(Status400BadRequest)]
    [ProducesResponseType(Status401Unauthorized)]
    [ProducesResponseType(Status404NotFound)]
    [HttpPut("{animalId}/locations"), Authorize]
    public async Task<UpdateVisit.Response> UpdateVisitLocation(
        [FromRoute] long animalId, [FromBody] UpdateVisit.Command command) =>
        await Mediator.Send(command with { Id = animalId });

    [ProducesResponseType(Status200OK)]
    [ProducesResponseType(Status400BadRequest)]
    [ProducesResponseType(Status401Unauthorized)]
    [ProducesResponseType(Status404NotFound)]
    [HttpDelete("{animalId}/locations/{visitId}"), Authorize]
    public async Task RemoveVisit([FromRoute] long animalId, [FromRoute] long visitId) =>
        await Mediator.Send(new RemoveVisit.Command(animalId, visitId));

    #endregion
}