using MediatR;

namespace DripChip.Application.Features.Animals.Commands.ReplaceType;

public record ReplaceTypeOfAnimalCommand(
    long Id,
    long OldTypeId,
    long NewTypeId) : IRequest<ReplaceTypeOfAnimalResponse>;