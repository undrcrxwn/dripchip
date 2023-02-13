using MediatR;

namespace DripChip.Application.Features.Animals.Queries.Search;

public record SearchAnimalQuery(
    
    DateTimeOffset? StartDateTime = default,
    DateTimeOffset? EndDateTime = default,
    
    int? ChipperId = default,
    long? ChippingLocationId = default,
    
    string? LifeStatus = default,
    string? Gender = default,
    
    int From = 0,
    int Size = 10
    
) : IRequest<IEnumerable<SearchAnimalResponse>>;