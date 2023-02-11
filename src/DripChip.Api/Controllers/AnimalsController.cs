using DripChip.Application.Features.Animals.Commands.Create;
using DripChip.Application.Features.Animals.Queries.GetById;
using DripChip.Application.Features.Animals.Queries.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DripChip.Api.Controllers;

public class AnimalsController : ApiControllerBase
{
    [HttpGet("{animalId}")]
    public async Task<GetAnimalByIdResponse> GetById([FromRoute] long animalId) =>
        await Mediator.Send(new GetAnimalByIdQuery(animalId));
    
    [HttpGet("[action]")]
    public async Task<IEnumerable<SearchAnimalResponse>> Search([FromRoute] SearchAnimalQuery query) =>
        await Mediator.Send(query);

    [HttpPost, Authorize]
    public async Task<IActionResult> Create([FromBody] CreateAnimalCommand command)
    {
        var response = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { AnimalId = response.Id }, response);
    }

    /*[HttpPut("{animalId}"), Authorize]
    public async Task<UpdateAnimalResponse> Update([FromRoute] long animalId, [FromBody] UpdateAnimalCommand command) =>
        await Mediator.Send(command with { Id = animalId });
    
    [HttpDelete("{animalId}"), Authorize]
    public async Task Delete([FromRoute] long animalId) =>
        await Mediator.Send(new DeleteAnimalCommand(animalId));*/
}