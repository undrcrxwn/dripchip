using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using MediatR;

namespace DripChip.Application.Features.AnimalTypes.Commands.Delete;

public class DeleteAnimalTypeCommandHandler : IRequestHandler<DeleteAnimalTypeCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteAnimalTypeCommandHandler(IApplicationDbContext context) =>
        _context = context;
    
    public async Task<Unit> Handle(DeleteAnimalTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.AnimalTypes.FindAsync(request.Id);

        if (entity is null)
            throw new NotFoundException();

        _context.AnimalTypes.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}