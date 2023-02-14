using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DripChip.Application.Features.AnimalTypes.Commands.Delete;

public class DeleteAnimalTypeCommandHandler : IRequestHandler<DeleteAnimalTypeCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteAnimalTypeCommandHandler(IApplicationDbContext context) =>
        _context = context;

    public async Task<Unit> Handle(DeleteAnimalTypeCommand request, CancellationToken cancellationToken)
    {
        var animalType =
            await _context.AnimalTypes
                .Include(type => type.Animals)
                .FirstOrDefaultAsync(type => type.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException();
        
        if (animalType.Animals.Any())
            throw new ValidationException(nameof(request.Id), "The specified animal type is attached to one or more animals.");

        _context.AnimalTypes.Remove(animalType);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}