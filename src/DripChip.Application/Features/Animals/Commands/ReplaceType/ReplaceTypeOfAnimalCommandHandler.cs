using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using Mapster;
using MediatR;

namespace DripChip.Application.Features.Animals.Commands.ReplaceType;

public class ReplaceTypeOfAnimalCommandHandler : IRequestHandler<ReplaceTypeOfAnimalCommand, ReplaceTypeOfAnimalResponse>
{
    private readonly IApplicationDbContext _context;

    public ReplaceTypeOfAnimalCommandHandler(IApplicationDbContext context) =>
        _context = context;

    public async Task<ReplaceTypeOfAnimalResponse> Handle(ReplaceTypeOfAnimalCommand request, CancellationToken cancellationToken)
    {
        var animal =
            await _context.Animals.FindAsync(request.Id)
            ?? throw new NotFoundException();

        var newAnimalType =
            await _context.AnimalTypes.FindAsync(request.NewTypeId)
            ?? throw new NotFoundException();

        if (animal.AnimalTypes.Contains(newAnimalType))
            throw new AlreadyExistsException();

        var oldAnimalType = animal.AnimalTypes.SingleOrDefault(animalType => animalType.Id == request.OldTypeId);
        if (oldAnimalType is null)
            throw new NotFoundException();
        
        animal.AnimalTypes.Remove(oldAnimalType);
        animal.AnimalTypes.Add(newAnimalType);

        await _context.SaveChangesAsync(cancellationToken);
        return animal.Adapt<ReplaceTypeOfAnimalResponse>();
    }
}