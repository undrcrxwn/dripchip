using MediatR;

namespace DripChip.Application.Features.Animals.Queries.SearchLocations;

public record SearchAnimalLocationsQuery(
    
    long Id,
    
    DateTimeOffset? StartDateTime = default,
    DateTimeOffset? EndDateTime = default,
    
    int From = 0,
    int Size = 10
    
) : IRequest<IEnumerable<SearchAnimalLocationsResponse>>;