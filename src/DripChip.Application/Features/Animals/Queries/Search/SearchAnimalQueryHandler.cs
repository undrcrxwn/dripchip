using DripChip.Application.Abstractions.Persistence;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DripChip.Application.Features.Animals.Queries.Search;

public class SearchAnimalQueryHandler : IRequestHandler<SearchAnimalQuery, IEnumerable<SearchAnimalResponse>>
{
    private readonly IApplicationDbContext _context;

    public SearchAnimalQueryHandler(IApplicationDbContext context) =>
        _context = context;

    public async Task<IEnumerable<SearchAnimalResponse>> Handle(SearchAnimalQuery request, CancellationToken cancellationToken)
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
            .Where(x =>
                request.ChipperId == null ||
                x.Chipper.Id == request.ChipperId)
            .Where(x =>
                request.ChippingLocationId == null ||
                x.ChippingLocation.Id == request.ChippingLocationId)
            .Where(x =>
                request.LifeStatus == null ||
                string.Equals(x.LifeStatus.ToString(), request.LifeStatus,
                    StringComparison.OrdinalIgnoreCase))
            .Where(x =>
                request.Gender == null ||
                string.Equals(x.Gender.ToString(), request.Gender,
                    StringComparison.OrdinalIgnoreCase))
            // Pagination
            .OrderBy(x => x.Id)
            .Skip(request.From)
            .Take(request.Size);

        return await animals
            .ProjectToType<SearchAnimalResponse>()
            .ToListAsync(cancellationToken);
    }
}