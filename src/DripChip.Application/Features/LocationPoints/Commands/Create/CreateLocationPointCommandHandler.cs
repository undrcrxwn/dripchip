using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using DripChip.Domain.Entities;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DripChip.Application.Features.LocationPoints.Commands.Create;

public class CreateLocationPointCommandHandler : IRequestHandler<CreateLocationPointCommand, CreateLocationPointResponse>
{
    private readonly IApplicationDbContext _context;

    public CreateLocationPointCommandHandler(IApplicationDbContext context) =>
        _context = context;

    public async Task<CreateLocationPointResponse> Handle(CreateLocationPointCommand request, CancellationToken cancellationToken)
    {
        var sameExists = await _context.LocationPoints.AnyAsync(x =>
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            x.Latitude == request.Latitude &&
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            x.Longitude == request.Longitude);

        if (sameExists)
            throw new AlreadyExistsException();
        
        var entity = request.Adapt<LocationPoint>();
        await _context.LocationPoints.AddAsync(entity);
        await _context.SaveChangesAsync(cancellationToken);
        
        return entity.Adapt<CreateLocationPointResponse>();
    }
}