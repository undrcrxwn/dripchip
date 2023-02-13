using DripChip.Application.Features.Animals.Commands.AddLocation;
using DripChip.Application.Features.Animals.Commands.AddType;
using DripChip.Application.Features.Animals.Commands.Create;
using DripChip.Application.Features.Animals.Commands.Delete;
using DripChip.Application.Features.Animals.Commands.RemoveType;
using DripChip.Application.Features.Animals.Commands.ReplaceLocation;
using DripChip.Application.Features.Animals.Commands.ReplaceType;
using DripChip.Application.Features.Animals.Commands.Update;
using DripChip.Application.Features.Animals.Queries.GetById;
using DripChip.Application.Features.Animals.Queries.Search;
using DripChip.Application.Features.Animals.Queries.SearchLocations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.Api.Controllers;

public class AnimalsController : ApiControllerBase
{
    [HttpGet("{animalId}")]
    public async Task<GetAnimalByIdResponse> GetById([FromRoute] long animalId) =>
        await Mediator.Send(new GetAnimalByIdQuery(animalId));

    [HttpGet("[action]")]
    public async Task<IEnumerable<SearchAnimalResponse>> Search([FromQuery] SearchAnimalQuery query) =>
        await Mediator.Send(query);

    [HttpPost, Authorize]
    public async Task<IActionResult> Create([FromBody] CreateAnimalCommand command)
    {
        var response = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { AnimalId = response.Id }, response);
    }

    [HttpPut("{animalId}"), Authorize]
    public async Task<UpdateAnimalResponse> Update([FromRoute] long animalId, [FromBody] UpdateAnimalCommand command) =>
        await Mediator.Send(command with { Id = animalId });

    [HttpDelete("{animalId}"), Authorize]
    public async Task Delete([FromRoute] long animalId) =>
        await Mediator.Send(new DeleteAnimalCommand(animalId));

    #region AnimalTypes

    [HttpPost("{animalId}/types/{typeId}"), Authorize]
    public async Task<IActionResult> AddType([FromRoute] long animalId, [FromRoute] long typeId)
    {
        var response = await Mediator.Send(new AddTypeToAnimalCommand(animalId, typeId));
        return CreatedAtAction(nameof(GetById), new { AnimalId = response.Id }, response);
    }

    [HttpPut("{animalId}/types"), Authorize]
    public async Task<ReplaceTypeOfAnimalResponse> ReplaceType([FromRoute] long animalId, [FromBody] ReplaceTypeOfAnimalCommand command) =>
        await Mediator.Send(command with { Id = animalId });

    [HttpDelete("{animalId}/types/{typeId}"), Authorize]
    public async Task<RemoveTypeFromAnimalResponse> RemoveType([FromRoute] long animalId, [FromRoute] long typeId) =>
        await Mediator.Send(new RemoveTypeFromAnimalCommand(animalId, typeId));

    #endregion

    #region VisitedLocations

    [HttpGet("{animalId}/locations")]
    public async Task<IEnumerable<SearchAnimalLocationsResponse>> SearchVisitedLocations(
        [FromRoute] long animalId, [FromQuery] SearchAnimalLocationsQuery query) =>
        await Mediator.Send(query with { Id = animalId });

    [HttpPost("{animalId}/locations/{pointId}")]
    public async Task<IActionResult> AddVisitedLocation([FromRoute] long animalId, [FromRoute] long pointId)
    {
        var response = await Mediator.Send(new AddLocationToAnimalCommand(animalId, pointId));
        return CreatedAtAction(nameof(GetById), new { AnimalId = response.Id }, response);
    }

    [HttpPut("{animalId}/locations")]
    public async Task<ReplaceLocationOfAnimalResponse> ReplaceVisitedLocation(
        [FromRoute] long animalId, [FromBody] ReplaceLocationOfAnimalCommand command) =>
        await Mediator.Send(command with { Id = animalId });

    #endregion
}