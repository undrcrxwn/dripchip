using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using Mapster;
using MediatR;

namespace DripChip.Application.Features.Animals.Commands.AddType;

public class AddTypeToAnimalCommandHandler : IRequestHandler<AddTypeToAnimalCommand, AddTypeToAnimalResponse>
{
    private readonly IApplicationDbContext _context;

    public AddTypeToAnimalCommandHandler(IApplicationDbContext context) =>
        _context = context;

    public async Task<AddTypeToAnimalResponse> Handle(AddTypeToAnimalCommand request, CancellationToken cancellationToken)
    {
        var animal =
            await _context.Animals.FindAsync(request.AnimalId)
            ?? throw new NotFoundException();

        var animalType =
            await _context.AnimalTypes.FindAsync(request.AnimalTypeId)
            ?? throw new NotFoundException();

        if (!animal.AnimalTypes.Contains(animalType))
            animal.AnimalTypes.Add(animalType);

        await _context.SaveChangesAsync(cancellationToken);
        return animal.Adapt<AddTypeToAnimalResponse>();
    }
}