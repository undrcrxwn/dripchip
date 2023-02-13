using MediatR;

namespace DripChip.Application.Features.Animals.Commands.AddLocation;

public record AddLocationToAnimalCommand(
    long AnimalId,
    long LocationPointId) : IRequest<AddLocationToAnimalResponse>;