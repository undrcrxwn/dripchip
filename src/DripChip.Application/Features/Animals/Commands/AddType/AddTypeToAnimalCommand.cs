using MediatR;

namespace DripChip.Application.Features.Animals.Commands.AddType;

public record AddTypeToAnimalCommand(
    long AnimalId,
    long AnimalTypeId) : IRequest<AddTypeToAnimalResponse>;