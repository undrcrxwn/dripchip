using MediatR;

namespace DripChip.Application.Features.AnimalTypes.Commands.Create;

public record CreateAnimalTypeCommand(string Type) : IRequest<CreateAnimalTypeResponse>;