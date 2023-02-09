using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using DripChip.Domain.Entities;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DripChip.Application.Features.LocationPoints.Commands.Update;

public class UpdateLocationPointCommandHandler : IRequestHandler<UpdateLocationPointCommand, UpdateLocationPointResponse>
{
    private readonly IApplicationDbContext _context;

    public UpdateLocationPointCommandHandler(IApplicationDbContext context) =>
        _context = context;

    public async Task<UpdateLocationPointResponse> Handle(UpdateLocationPointCommand request, CancellationToken cancellationToken)
    {
        var exists = await _context.LocationPoints.AnyAsync(x => x.Id == request.Id);

        if (!exists)
            throw new NotFoundException();

        var entity = request.Adapt<LocationPoint>();
        _context.LocationPoints.Update(request.Adapt<LocationPoint>());
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Adapt<UpdateLocationPointResponse>();
    }
}