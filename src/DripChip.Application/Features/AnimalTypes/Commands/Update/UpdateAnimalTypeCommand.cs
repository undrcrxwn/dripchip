using MediatR;

namespace DripChip.Application.Features.AnimalTypes.Commands.Update;

public record UpdateAnimalTypeCommand(long Id, string Type) : IRequest<UpdateAnimalTypeResponse>;