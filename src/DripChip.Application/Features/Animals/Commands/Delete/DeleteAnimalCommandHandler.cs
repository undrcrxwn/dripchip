using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DripChip.Application.Features.Animals.Commands.Delete;

public class DeleteAnimalCommandHandler : IRequestHandler<DeleteAnimalCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteAnimalCommandHandler(IApplicationDbContext context) =>
        _context = context;

    public async Task<Unit> Handle(DeleteAnimalCommand request, CancellationToken cancellationToken)
    {
        var animal =
            await _context.Animals
                .Include(animal => animal.VisitedLocations)
                .FirstOrDefaultAsync(animal => animal.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException();

        if (animal.VisitedLocations.Any())
            throw new ValidationException(nameof(request.Id), "The specified animal has moved away from its chipping location.");

        _context.Animals.Remove(animal);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}