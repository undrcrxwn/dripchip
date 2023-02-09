using MediatR;

namespace DripChip.Application.Features.Animals.Queries.Search;

public record SearchAnimalQuery(
    
    DateTime? StartDateTime = default,
    DateTime? EndDateTime = default,
    
    int? ChipperId = default,
    long? ChippingLocationId = default,
    
    string? LifeStatus = default,
    string? Gender = default,
    
    int From = 0,
    int Size = 10
    
) : IRequest<IEnumerable<SearchAnimalResponse>>;