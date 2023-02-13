using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using Mapster;
using MediatR;

namespace DripChip.Application.Features.Animals.Commands.RemoveType;

public class RemoveTypeFromAnimalCommandHandler : IRequestHandler<RemoveTypeFromAnimalCommand, RemoveTypeFromAnimalResponse>
{
    private readonly IApplicationDbContext _context;

    public RemoveTypeFromAnimalCommandHandler(IApplicationDbContext context) =>
        _context = context;

    public async Task<RemoveTypeFromAnimalResponse> Handle(RemoveTypeFromAnimalCommand request, CancellationToken cancellationToken)
    {
        var animal =
            await _context.Animals.FindAsync(request.AnimalId)
            ?? throw new NotFoundException();

        var animalType =
            await _context.AnimalTypes.FindAsync(request.AnimalTypeId)
            ?? throw new NotFoundException();
        
        if (!animal.AnimalTypes.Contains(animalType))
            throw new NotFoundException();

        if (animal.AnimalTypes.Count == 1)
            throw new ValidationException(nameof(request.AnimalTypeId),
                "The specified animal type is the only type attached to the animal.");
        
        animal.AnimalTypes.Remove(animalType);

        await _context.SaveChangesAsync(cancellationToken);
        return animal.Adapt<RemoveTypeFromAnimalResponse>();
    }
}