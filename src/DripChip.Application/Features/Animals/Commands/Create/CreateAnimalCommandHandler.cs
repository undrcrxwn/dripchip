using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using DripChip.Domain.Entities;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DripChip.Application.Features.Animals.Commands.Create;

public class CreateAnimalCommandHandler : IRequestHandler<CreateAnimalCommand, CreateAnimalResponse>
{
    private readonly IApplicationDbContext _context;

    public CreateAnimalCommandHandler(IApplicationDbContext context) =>
        _context = context;

    public async Task<CreateAnimalResponse> Handle(CreateAnimalCommand request, CancellationToken cancellationToken)
    {
        // Animal types validation
        var animalTypes = await _context.AnimalTypes.Where(animalType =>
            request.AnimalTypes.Contains(animalType.Id))
            .ToListAsync(cancellationToken);

        if (request.AnimalTypes.Count() > animalTypes.Count)
            throw new NotFoundException("Some of the specified animal types does not exist.");
        
        // Chipper ID validation
        var chipper =
            await _context.Accounts.FindAsync(request.ChipperId)
            ?? throw new NotFoundException(nameof(Account), nameof(request.ChipperId));
        
        // Chipping location point ID validation
        var chippingLocationPoint =
            await _context.LocationPoints.FindAsync(request.ChippingLocationId)
            ?? throw new NotFoundException(nameof(LocationPoint), request.ChippingLocationId);
        
        var entity = request.Adapt<Animal>();
        entity.AnimalTypes = animalTypes;
        entity.Chipper = chipper;
        entity.ChippingLocation = chippingLocationPoint;
        entity.ChippingDateTime = DateTime.UtcNow;

        await _context.Animals.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Adapt<CreateAnimalResponse>();
    }
}