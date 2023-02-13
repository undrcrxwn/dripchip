using MediatR;

namespace DripChip.Application.Features.Animals.Commands.RemoveType;

public record RemoveTypeFromAnimalCommand(
    long AnimalId,
    long AnimalTypeId) : IRequest<RemoveTypeFromAnimalResponse>;