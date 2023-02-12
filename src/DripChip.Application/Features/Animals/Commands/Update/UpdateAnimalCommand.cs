using MediatR;

namespace DripChip.Application.Features.Animals.Commands.Update;

public record UpdateAnimalCommand(
    long Id,
    float Weight,
    float Length,
    float Height,
    string Gender,
    string LifeStatus,
    int ChipperId,
    long ChippingLocationId) : IRequest<UpdateAnimalResponse>;