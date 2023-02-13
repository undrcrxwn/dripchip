using MediatR;

namespace DripChip.Application.Features.Animals.Commands.ReplaceLocation;

public record ReplaceLocationOfAnimalCommand(
    long Id,
    long VisitedLocationPointId,
    long LocationPointId) : IRequest<ReplaceLocationOfAnimalResponse>;