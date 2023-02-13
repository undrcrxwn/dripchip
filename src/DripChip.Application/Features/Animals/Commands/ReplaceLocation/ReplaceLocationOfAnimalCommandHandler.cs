using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using Mapster;
using MediatR;

namespace DripChip.Application.Features.Animals.Commands.ReplaceLocation;

public class ReplaceLocationOfAnimalCommandHandler : IRequestHandler<ReplaceLocationOfAnimalCommand, ReplaceLocationOfAnimalResponse>
{
    private readonly IApplicationDbContext _context;

    public ReplaceLocationOfAnimalCommandHandler(IApplicationDbContext context) =>
        _context = context;

    public async Task<ReplaceLocationOfAnimalResponse> Handle(ReplaceLocationOfAnimalCommand request, CancellationToken cancellationToken)
    {
        var animal =
            await _context.Animals.FindAsync(request.Id)
            ?? throw new NotFoundException();

        var newLocationPoint =
            await _context.LocationPoints.FindAsync(request.LocationPointId)
            ?? throw new NotFoundException();

        var oldLocationPoint = animal.VisitedLocations.SingleOrDefault(locationPoint =>
            locationPoint.Id == request.LocationPointId);

        if (oldLocationPoint is null)
            throw new NotFoundException();

        var visitedLocationPointIndex = animal.VisitedLocations.IndexOf(oldLocationPoint);
        
        animal.VisitedLocations.RemoveAt(visitedLocationPointIndex);
        animal.VisitedLocations.Insert(visitedLocationPointIndex, newLocationPoint);

        await _context.SaveChangesAsync(cancellationToken);
        return animal.Adapt<ReplaceLocationOfAnimalResponse>();
    }
}