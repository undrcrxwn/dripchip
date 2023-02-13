using MediatR;

namespace DripChip.Application.Features.Animals.Queries.SearchLocations;

public record SearchAnimalLocationsQuery(
    
    long Id,
    
    DateTime? StartDateTime = default,
    DateTime? EndDateTime = default,
    
    int From = 0,
    int Size = 10
    
) : IRequest<IEnumerable<SearchAnimalLocationsResponse>>;