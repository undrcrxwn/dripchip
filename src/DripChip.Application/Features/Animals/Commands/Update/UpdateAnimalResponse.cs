namespace DripChip.Application.Features.Animals.Commands.Update;

public record UpdateAnimalResponse(
    long Id,
    float Weight,
    float Length,
    float Height,
    string Gender,
    string LifeStatus,
    int ChipperId,
    long ChippingLocationId);