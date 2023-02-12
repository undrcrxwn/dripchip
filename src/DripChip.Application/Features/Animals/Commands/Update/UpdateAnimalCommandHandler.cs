using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using DripChip.Domain.Entities;
using DripChip.Domain.Enumerations;
using Mapster;
using MediatR;

namespace DripChip.Application.Features.Animals.Commands.Update;

public class UpdateAnimalCommandHandler : IRequestHandler<UpdateAnimalCommand, UpdateAnimalResponse>
{
    private readonly IApplicationDbContext _context;

    public UpdateAnimalCommandHandler(IApplicationDbContext context) =>
        _context = context;

    public async Task<UpdateAnimalResponse> Handle(UpdateAnimalCommand request, CancellationToken cancellationToken)
    {
        var animal =
            await _context.Animals.FindAsync(request.Id)
            ?? throw new NotFoundException();

        var modifiedAnimal = request.Adapt<Animal>();

        if (animal.LifeStatus == AnimalLifeStatus.Dead && modifiedAnimal.LifeStatus != AnimalLifeStatus.Dead)
            throw new ValidationException(nameof(request.LifeStatus), "The life status of a dead animal cannot be changed.");

        if (animal.LocationPoints.FirstOrDefault()?.Id == request.ChippingLocationId)
            throw new ValidationException(nameof(request.ChippingLocationId),
                "The specified chipping location point matches the animal's first visited location.");

        _context.Animals.Attach(modifiedAnimal);
        _context.Animals.Update(modifiedAnimal);

        await _context.SaveChangesAsync(cancellationToken);
        return animal.Adapt<UpdateAnimalResponse>();
    }
}