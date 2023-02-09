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
        var entity = request.Adapt<LocationPoint>();

        var sameExists = await _context.LocationPoints.AnyAsync(x =>
            x.Latitude == request.Latitude && x.Longitude == request.Longitude);

        if (sameExists)
            throw new AlreadyExistsException();
        
        await _context.LocationPoints.AddAsync(entity);
        await _context.SaveChangesAsync(cancellationToken);
        
        return entity.Adapt<CreateLocationPointResponse>();
    }
}