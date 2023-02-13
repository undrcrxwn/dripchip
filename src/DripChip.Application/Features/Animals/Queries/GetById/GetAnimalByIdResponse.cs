namespace DripChip.Application.Features.Animals.Queries.GetById;

public record GetAnimalByIdResponse(
    long Id,
    IEnumerable<long> AnimalTypes,
    float Weight,
    float Length,
    float Height,
    string Gender,
    string LifeStatus,
    DateTimeOffset ChippingDateTime,
    int ChipperId,
    long ChippingLocationId,
    IEnumerable<long> VisitedLocations,
    DateTimeOffset? DeathDateTime);