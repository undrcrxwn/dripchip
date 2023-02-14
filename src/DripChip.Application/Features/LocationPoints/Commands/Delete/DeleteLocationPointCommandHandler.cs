using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DripChip.Application.Features.LocationPoints.Commands.Delete;

public class DeleteLocationPointCommandHandler : IRequestHandler<DeleteLocationPointCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteLocationPointCommandHandler(IApplicationDbContext context) =>
        _context = context;

    public async Task<Unit> Handle(DeleteLocationPointCommand request, CancellationToken cancellationToken)
    {
        var locationPoint =
            await _context.LocationPoints
                .Include(point => point.ChippedAnimals)
                .Include(point => point.Visitors)
                .FirstOrDefaultAsync(point => point.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException();
        
        if (locationPoint.ChippedAnimals.Any())
            throw new ValidationException(nameof(request.Id), "The specified location is a chipping point for one or more animals.");
        
        if (locationPoint.Visitors.Any())
            throw new ValidationException(nameof(request.Id), "The specified location is a visited point for one or more animals.");

        _context.LocationPoints.Remove(locationPoint);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}