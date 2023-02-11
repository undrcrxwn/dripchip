using MediatR;

namespace DripChip.Application.Features.Animals.Commands.Create;

public record CreateAnimalCommand(
    IEnumerable<long> AnimalTypes,
    float Weight,
    float Length,
    float Height,
    string Gender,
    string LifeStatus,
    DateTime ChippingDateTime,
    int ChipperId,
    long ChippingLocationId) : IRequest<CreateAnimalResponse>;