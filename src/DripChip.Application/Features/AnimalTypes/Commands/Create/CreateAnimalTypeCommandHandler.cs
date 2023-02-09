using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using DripChip.Domain.Entities;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DripChip.Application.Features.AnimalTypes.Commands.Create;

public class CreateAnimalTypeCommandHandler : IRequestHandler<CreateAnimalTypeCommand, CreateAnimalTypeResponse>
{
    private readonly IApplicationDbContext _context;

    public CreateAnimalTypeCommandHandler(IApplicationDbContext context) =>
        _context = context;

    public async Task<CreateAnimalTypeResponse> Handle(CreateAnimalTypeCommand request, CancellationToken cancellationToken)
    {
        var sameExists = await _context.AnimalTypes.AnyAsync(x =>
            x.Type == request.Type);

        if (sameExists)
            throw new AlreadyExistsException();
        
        var entity = request.Adapt<AnimalType>();
        await _context.AnimalTypes.AddAsync(entity);
        await _context.SaveChangesAsync(cancellationToken);
        
        return entity.Adapt<CreateAnimalTypeResponse>();
    }
}