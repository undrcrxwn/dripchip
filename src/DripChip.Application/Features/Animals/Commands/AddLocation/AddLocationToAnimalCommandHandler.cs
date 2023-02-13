using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using DripChip.Domain.Enumerations;
using Mapster;
using MediatR;

namespace DripChip.Application.Features.Animals.Commands.AddLocation;

public class AddLocationToAnimalCommandHandler : IRequestHandler<AddLocationToAnimalCommand, AddLocationToAnimalResponse>
{
    private readonly IApplicationDbContext _context;

    public AddLocationToAnimalCommandHandler(IApplicationDbContext context) =>
        _context = context;

    public async Task<AddLocationToAnimalResponse> Handle(AddLocationToAnimalCommand request, CancellationToken cancellationToken)
    {
        var animal =
            await _context.Animals.FindAsync(request.AnimalId)
            ?? throw new NotFoundException();

        var locationPoint =
            await _context.LocationPoints.FindAsync(request.LocationPointId)
            ?? throw new NotFoundException();

        if (animal.ChippingLocation == locationPoint)
            throw new ValidationException(nameof(request.LocationPointId),
                "The specified location point matches the animal's chipping location point.");

        if (animal.LifeStatus == AnimalLifeStatus.Dead)
            throw new ValidationException(nameof(request.LocationPointId),
                "The animal's location cannot be changed, since the animal is dead.");

        if (animal.VisitedLocations.Any() && animal.VisitedLocations.Last() == locationPoint)
            throw new ValidationException(nameof(request.LocationPointId),
                "The specified location point matches the animal's current location point.");

        animal.VisitedLocations.Add(locationPoint);

        await _context.SaveChangesAsync(cancellationToken);
        return animal.Adapt<AddLocationToAnimalResponse>() with { DateTimeOfVisitLocationPoint = DateTimeOffset.Now };
    }
}