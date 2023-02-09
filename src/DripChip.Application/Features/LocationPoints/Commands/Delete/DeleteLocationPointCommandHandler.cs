using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using MediatR;

namespace DripChip.Application.Features.LocationPoints.Commands.Delete;

public class DeleteLocationPointCommandHandler : IRequestHandler<DeleteLocationPointCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteLocationPointCommandHandler(IApplicationDbContext context) =>
        _context = context;
    
    public async Task<Unit> Handle(DeleteLocationPointCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.LocationPoints.FindAsync(request.Id);

        if (entity is null)
            throw new NotFoundException();

        _context.LocationPoints.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}