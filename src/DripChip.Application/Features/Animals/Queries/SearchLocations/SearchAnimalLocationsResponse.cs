namespace DripChip.Application.Features.Animals.Queries.SearchLocations;

public record SearchAnimalLocationsResponse(
    long Id,
    long LocationPointId,
    DateTimeOffset DateTimeOfVisitLocationPoint);