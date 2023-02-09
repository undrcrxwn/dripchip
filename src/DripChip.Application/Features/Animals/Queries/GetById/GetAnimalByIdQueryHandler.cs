using DripChip.Application.Abstractions.Persistence;
using DripChip.Application.Exceptions;
using Mapster;
using MediatR;

namespace DripChip.Application.Features.Animals.Queries.GetById;

public class GetAnimalByIdQueryHandler : IRequestHandler<GetAnimalByIdQuery, GetAnimalByIdResponse>
{
    private readonly IApplicationDbContext _context;

    public GetAnimalByIdQueryHandler(IApplicationDbContext context) =>
        _context = context;
    
    public async Task<GetAnimalByIdResponse> Handle(GetAnimalByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Animals.FindAsync(request.Id);

        if (entity is null)
            throw new NotFoundException();
        
        return entity.Adapt<GetAnimalByIdResponse>();
    }
}