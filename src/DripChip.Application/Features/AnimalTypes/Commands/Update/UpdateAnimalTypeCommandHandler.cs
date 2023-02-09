using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using DripChip.Domain.Entities;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DripChip.Application.Features.AnimalTypes.Commands.Update;

public class UpdateAnimalTypeCommandHandler : IRequestHandler<UpdateAnimalTypeCommand, UpdateAnimalTypeResponse>
{
    private readonly IApplicationDbContext _context;

    public UpdateAnimalTypeCommandHandler(IApplicationDbContext context) =>
        _context = context;

    public async Task<UpdateAnimalTypeResponse> Handle(UpdateAnimalTypeCommand request, CancellationToken cancellationToken)
    {
        var exists = await _context.AnimalTypes.AnyAsync(x =>
            x.Id == request.Id, cancellationToken: cancellationToken);

        if (!exists)
            throw new NotFoundException();
        
        var sameExists = await _context.AnimalTypes.AnyAsync(x =>
            x.Type == request.Type, cancellationToken);

        if (sameExists)
            throw new AlreadyExistsException();


        var entity = request.Adapt<AnimalType>();
        _context.AnimalTypes.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Adapt<UpdateAnimalTypeResponse>();
    }
}