using DripChip.Application.Abstractions.Persistence;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DripChip.Application.Features.Animals.Queries.SearchLocations;

public class SearchAnimalLocationsQueryHandler : IRequestHandler<SearchAnimalLocationsQuery, IEnumerable<SearchAnimalLocationsResponse>>
{
    private readonly IApplicationDbContext _context;

    public SearchAnimalLocationsQueryHandler(IApplicationDbContext context) =>
        _context = context;

    public async Task<IEnumerable<SearchAnimalLocationsResponse>> Handle(SearchAnimalLocationsQuery request, CancellationToken cancellationToken)
    {
        var animals = _context.Animals
            .Include(animal => animal.AnimalTypes)
            .Include(animal => animal.VisitedLocations)
            // Filtering
            .Where(x =>
                request.StartDateTime == null ||
                x.ChippingDateTime > request.StartDateTime)
            .Where(x =>
                request.EndDateTime == null ||
                x.ChippingDateTime < request.EndDateTime)
            // Pagination
            .OrderBy(x => x.Id)
            .Skip(request.From)
            .Take(request.Size);

        return await animals
            .ProjectToType<SearchAnimalLocationsResponse>()
            .ToListAsync(cancellationToken);
    }
}