using DripChip.Api.Attributes;
using DripChip.Application.Features.AnimalTypes.Commands.Create;
using DripChip.Application.Features.AnimalTypes.Commands.Delete;
using DripChip.Application.Features.AnimalTypes.Commands.Update;
using DripChip.Application.Features.AnimalTypes.Queries.GetById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.Api.Controllers;

[ApiRoute("animals/types")]
public class AnimalTypesController : ApiControllerBase
{
    [HttpGet("{typeId}")]
    public async Task<GetAnimalTypeByIdResponse> GetById([FromRoute] long typeId) =>
        await Mediator.Send(new GetAnimalTypeByIdQuery(typeId));

    [HttpPost, Authorize]
    public async Task<IActionResult> Create([FromBody] CreateAnimalTypeCommand command)
    {
        var response = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { TypeId = response.Id }, response);
    }

    [HttpPut("{typeId}"), Authorize]
    public async Task<UpdateAnimalTypeResponse> Update([FromRoute] long typeId, [FromBody] UpdateAnimalTypeCommand command) =>
        await Mediator.Send(command with { Id = typeId });
    
    [HttpDelete("{typeId}"), Authorize]
    public async Task Delete([FromRoute] long typeId) =>
        await Mediator.Send(new DeleteAnimalTypeCommand(typeId));
}