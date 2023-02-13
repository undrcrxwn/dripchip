using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using MediatR;

namespace DripChip.Application.Features.Animals.Commands.Delete;

public class DeleteAnimalCommandHandler : IRequestHandler<DeleteAnimalCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteAnimalCommandHandler(IApplicationDbContext context) =>
        _context = context;

    public async Task<Unit> Handle(DeleteAnimalCommand request, CancellationToken cancellationToken)
    {
        var animal =
            await _context.Animals.FindAsync(request.Id)
            ?? throw new NotFoundException();

        _context.Animals.Remove(animal);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}