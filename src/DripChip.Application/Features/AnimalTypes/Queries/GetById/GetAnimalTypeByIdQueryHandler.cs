using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using Mapster;
using MediatR;

namespace DripChip.Application.Features.AnimalTypes.Queries.GetById;

public class GetAnimalTypeByIdQueryHandler : IRequestHandler<GetAnimalTypeByIdQuery, GetAnimalTypeByIdResponse>
{
    private readonly IApplicationDbContext _context;

    public GetAnimalTypeByIdQueryHandler(IApplicationDbContext context) =>
        _context = context;
    
    public async Task<GetAnimalTypeByIdResponse> Handle(GetAnimalTypeByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.AnimalTypes.FindAsync(request.Id);

        if (entity is null)
            throw new NotFoundException();
        
        return entity.Adapt<GetAnimalTypeByIdResponse>();
    }
}